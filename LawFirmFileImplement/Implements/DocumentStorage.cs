using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmFileImplement.Implements
{
    public class DocumentStorage : IDocumentStorage
    {
        private readonly FileDataListSingleton source;
        public DocumentStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public List<DocumentViewModel> GetFullList()
        {
            return source.Documents.Select(CreateModel)
            .ToList();
        }
        public List<DocumentViewModel> GetFilteredList(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Documents
            .Where(rec => rec.DocumentName.Contains(model.DocumentName))
            .Select(CreateModel)
            .ToList();
        }
        public DocumentViewModel GetElement(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var document = source.Documents
  .FirstOrDefault(rec => rec.DocumentName == model.DocumentName || rec.Id
 == model.Id);
            return document != null ? CreateModel(document) : null;
        }
        public void Insert(DocumentBindingModel model)
        {

            int maxId = source.Documents.Count > 0 ? source.Blanks.Max(rec => rec.Id)
    : 0;
            var element = new Document
            {
                Id = maxId + 1,
                DocumentBlanks = new
           Dictionary<int, int>()
            };
            source.Documents.Add(CreateModel(model, element));
        }
        public void Update(DocumentBindingModel model)
        {
            var element = source.Documents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
        }
        public void Delete(DocumentBindingModel model)
        {
            Document element = source.Documents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Documents.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
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
            return new DocumentViewModel

            {
                Id = document.Id,
                DocumentName = document.DocumentName,
                Price = document.Price,
                DocumentBlanks = document.DocumentBlanks
 .ToDictionary(recPC => recPC.Key, recPC =>
 (source.Blanks.FirstOrDefault(recC => recC.Id ==
recPC.Key)?.BlankName, recPC.Value))
            };
        }
    }

}
