using System;
using System.Collections.Generic;
using System.Text;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.ViewModels;

namespace LawFirmBusinessLogic.Interfaces
{
    public interface IDocumentStorage
    {
        List<DocumentViewModel> GetFullList();
        List<DocumentViewModel> GetFilteredList(DocumentBindingModel model);
        DocumentViewModel GetElement(DocumentBindingModel model);
        void Insert(DocumentBindingModel model);
        void Update(DocumentBindingModel model);
        void Delete(DocumentBindingModel model);
    }
}
