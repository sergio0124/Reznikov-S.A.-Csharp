using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LawFirmListImplement.Implements
{
    public class DocumentStorage : IDocumentStorage
    {
        private readonly DataListSingleton source;
        public DocumentStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<DocumentViewModel> GetFullList()
        {
            List<DocumentViewModel> result = new List<DocumentViewModel>();
            foreach (var blank in source.Documents)
            {
                result.Add(CreateModel(blank));
            }
            return result;
        }
        public List<DocumentViewModel> GetFilteredList(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<DocumentViewModel> result = new List<DocumentViewModel>();
            foreach (var document in source.Documents)
            {
                if (document.DocumentName.Contains(model.DocumentName))
                {
                    result.Add(CreateModel(document));
                }
            }
            return result;
        }
        public DocumentViewModel GetElement(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var document in source.Documents)
            {
                if (document.Id == model.Id || document.DocumentName ==
                model.DocumentName)
                {
                    return CreateModel(document);
                }
            }
            return null;
        }

public void Insert(DocumentBindingModel model)
        {
            Document tempDocument = new Document
            {
                Id = 1,
                DocumentBlanks = new
            Dictionary<int, int>()
            };
            foreach (var product in source.Documents)
            {
                if (product.Id >= tempDocument.Id)
                {
                    tempDocument.Id = product.Id + 1;
                }
            }
            source.Documents.Add(CreateModel(model, tempDocument));
        }
        public void Update(DocumentBindingModel model)
        {
            Document tempDocument = null;
            foreach (var document in source.Documents)
            {
                if (document.Id == model.Id)
                {
                    tempDocument = document;
                }
            }
            if (tempDocument == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempDocument);
        }
        public void Delete(DocumentBindingModel model)
        {
            for (int i = 0; i < source.Documents.Count; ++i)
            {
                if (source.Documents[i].Id == model.Id)
                {
                    source.Documents.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Document CreateModel(DocumentBindingModel model, Document document)
        {
            document.DocumentName = model.DocumentName;
            document.Price = model.Price;
            // удаляем убранные
            foreach (var key in document.DocumentBlanks.Keys.ToList())
            {
                if (!model.DocumentBlanks.ContainsKey(key))
                {
                    document.DocumentBlanks.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var blank in model.DocumentBlanks)
            {
                if (document.DocumentBlanks.ContainsKey(blank.Key))
                {
                    document.DocumentBlanks[blank.Key] =
                    model.DocumentBlanks[blank.Key].Item2;
                    
                }
                else
                {
                    document.DocumentBlanks.Add(blank.Key,
                    model.DocumentBlanks[blank.Key].Item2);
                }
            }
            return document;
        }
        private DocumentViewModel CreateModel(Document document)
        {
            // требуется дополнительно получить список компонентов для изделия с
            //названиями и их количество
        Dictionary<int, (string, int)> productComponents = new
        Dictionary<int, (string, int)>();
            foreach (var db in document.DocumentBlanks)
            {
                string componentName = string.Empty;
                foreach (var component in source.Blanks)
                {
                    if (db.Key == component.Id)
                    {
                        componentName = component.BlankName;
                        break;
                    }
                }
                productComponents.Add(db.Key, (componentName, db.Value));
            }
            return new DocumentViewModel
            {
                Id = document.Id,
                DocumentName = document.DocumentName,
                Price = document.Price,
                DocumentBlanks = productComponents
            };
        }
    }
}