using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.Interfaces
{
    public interface IBlankStorage
    {
        List<BlankViewModel> GetFullList();
        List<BlankViewModel> GetFilteredList(BlankBindingModel model);
        BlankViewModel GetElement(BlankBindingModel model);
        void Insert(BlankBindingModel model);
        void Update(BlankBindingModel model);
        void Delete(BlankBindingModel model);
    }
}
