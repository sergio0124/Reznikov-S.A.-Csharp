using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class DocumentLogic
    {
        private readonly IDocumentStorage _documentStorage;
        public DocumentLogic(IDocumentStorage documentStorage)
        {
            _documentStorage = documentStorage;
        }

        public List<DocumentViewModel> Read(DocumentBindingModel model)
        {
            if (model == null)
            {
                return _documentStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<DocumentViewModel> { _documentStorage.GetElement(model) };
            }
            return _documentStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(DocumentBindingModel model)
        {
            var element = _documentStorage.GetElement(new DocumentBindingModel
            {
                DocumentName = model.DocumentName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть документ с таким названием");
            }
            if (model.Id.HasValue)
            {
                _documentStorage.Update(model);
            }
            else
            {
                _documentStorage.Insert(model);
            }
        }
        public void Delete(DocumentBindingModel model)

        {
            var element = _documentStorage.GetElement(new DocumentBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _documentStorage.Delete(model);
        }
       
    }
}
