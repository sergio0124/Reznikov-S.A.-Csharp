using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmDatabaseImplement.Implements
{
    public class DocumentStorage : IDocumentStorage
    {
        public List<DocumentViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Documents
                .Include(rec => rec.DocumentBlanks)
               .ThenInclude(rec => rec.Blank)
               .ToList()
               .Select(rec => new DocumentViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.DocumentName,
                   Price = rec.Price,
                   DocumentBlanks = rec.DocumentBlanks

                .ToDictionary(recPC => recPC.DocumentId, recPC =>
               (recPC.Document?.DocumentName, recPC.Count))
               })
               .ToList();
            }
        }
        public List<DocumentViewModel> GetFilteredList(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                return context.Documents
                .Include(rec => rec.DocumentBlanks)
               .ThenInclude(rec => rec.Document)
               .Where(rec => rec.DocumentName.Contains(model.DocumentName))
               .ToList()
               .Select(rec => new DocumentViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.DocumentName,
                   Price = rec.Price,
                   DocumentBlanks = rec.DocumentBlanks
                .ToDictionary(recPC => recPC.DocumentId, recPC =>
               (recPC.Document?.DocumentName, recPC.Count))
               })
               .ToList();
            }
        }
        public DocumentViewModel GetElement(DocumentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                var document = context.Documents
                .Include(rec => rec.DocumentBlanks)
               .ThenInclude(rec => rec.Document)
               .FirstOrDefault(rec => rec.DocumentName == model.DocumentName || rec.Id
               == model.Id);
                return document != null ?
                new DocumentViewModel
                {
                    Id = document.Id,
                    DocumentName = document.DocumentName,
                    Price = document.Price,
                    DocumentBlanks = document.DocumentBlanks
                .ToDictionary(recPC => recPC.DocumentId, recPC =>
               (recPC.Document?.DocumentName, recPC.Count))
                } :
               null;
            }
        }
        public void Insert(DocumentBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Documents.Add(CreateModel(model, new Document(), context));
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(DocumentBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Documents.FirstOrDefault(rec => rec.Id ==
                       model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(DocumentBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Document element = context.Documents.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Documents.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Document CreateModel(DocumentBindingModel model, Document document,
       LawFirmDatabase context)
        {
            document.DocumentName = model.DocumentName;
            document.Price = model.Price;
            if (model.Id.HasValue)
            {
                var documentBlanks = context.DocumentBlanks.Where(rec =>
               rec.DocumentId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.DocumentBlanks.RemoveRange(documentBlanks.Where(rec =>
               !model.DocumentBlanks.ContainsKey(rec.DocumentId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateBlank in documentBlanks)
                {
                    updateBlank.Count =
                   model.DocumentBlanks[updateBlank.DocumentId].Item2;
                    model.DocumentBlanks.Remove(updateBlank.DocumentId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.DocumentBlanks) 
        {
                context.DocumentBlanks.Add(new DocumentBlank
                {
                    DocumentId = document.Id,
                    BlankId = pc.Key,
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            return document;
        }

    }
}
