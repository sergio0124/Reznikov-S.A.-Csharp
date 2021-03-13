using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.Interfaces
{
    public interface IStorageStorage
    {
        List<StorageViewModel> GetFullList();
        List<StorageViewModel> GetFilteredList(StorageBindingModel model);
        StorageViewModel GetElement(StorageBindingModel model);
        void Insert(StorageBindingModel model);
        void Update(StorageBindingModel model);
        void Delete(StorageBindingModel model);
    }
}
