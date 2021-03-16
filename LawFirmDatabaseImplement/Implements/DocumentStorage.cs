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
							.ToDictionary(recDocumentBlanks => recDocumentBlanks.BlankId,
							recDocumentBlanks => (recDocumentBlanks.Blank?.BlankName,
							recDocumentBlanks.Count))
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
					.ThenInclude(rec => rec.Blank)
					.Where(rec => rec.DocumentName.Contains(model.DocumentName))
					.ToList()
					.Select(rec => new DocumentViewModel
					{
						Id = rec.Id,
						DocumentName = rec.DocumentName,
						Price = rec.Price,
						DocumentBlanks = rec.DocumentBlanks
							.ToDictionary(recDocumentBlanks => recDocumentBlanks.BlankId,
							recDocumentBlanks => (recDocumentBlanks.Blank?.BlankName,
							recDocumentBlanks.Count))
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
					.ThenInclude(rec => rec.Blank)
					.FirstOrDefault(rec => rec.DocumentName == model.DocumentName ||
					rec.Id == model.Id);


				return document != null ?
					new DocumentViewModel
					{
						Id = document.Id,
						DocumentName = document.DocumentName,
						Price = document.Price,
						DocumentBlanks = document.DocumentBlanks
							.ToDictionary(recDocumentBlank => recDocumentBlank.BlankId,
							recDocumentBlank => (recDocumentBlank.Blank?.BlankName,
							recDocumentBlank.Count))
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
						CreateModel(model, new Document(), context);
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
						var document = context.Documents.FirstOrDefault(rec => rec.Id == model.Id);


						if (document == null)
						{
							throw new Exception("Подарок не найден");
						}


						CreateModel(model, document, context);
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
				var blank = context.Documents.FirstOrDefault(rec => rec.Id == model.Id);


				if (blank == null)
				{
					throw new Exception("Материал не найден");
				}


				context.Documents.Remove(blank);
				context.SaveChanges();
			}
		}
		private Document CreateModel(DocumentBindingModel model, Document document, LawFirmDatabase context)
		{
			document.DocumentName = model.DocumentName;
			document.Price = model.Price;
			if (document.Id == 0)
			{
				context.Documents.Add(document);
				context.SaveChanges();
			}


			if (model.Id.HasValue)
			{
				var documentBlank = context.DocumentBlanks
					.Where(rec => rec.DocumentId == model.Id.Value)
					.ToList();


				context.DocumentBlanks.RemoveRange(documentBlank
					.Where(rec => !model.DocumentBlanks.ContainsKey(rec.DocumentId))
					.ToList());
				context.SaveChanges();


				foreach (var updateBlank in documentBlank)
				{
					updateBlank.Count = model.DocumentBlanks[updateBlank.BlankId].Item2;
					model.DocumentBlanks.Remove(updateBlank.DocumentId);
				}
				context.SaveChanges();
			}
			foreach (var documentBlank in model.DocumentBlanks)
			{
				context.DocumentBlanks.Add(new DocumentBlank
				{
					DocumentId = document.Id,
					BlankId = documentBlank.Key,
					Count = documentBlank.Value.Item2
				});
				context.SaveChanges();
			}
			return document;
		}



	}
}
