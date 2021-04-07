using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.Interfaces
{
    public interface IClientStorage
    {
        List<ClientViewModel> GetFullList();

        ClientViewModel GetElement(ClientBindingModel model);

        List<ClientViewModel> GetFilteredList(ClientBindingModel model);

        void Update(ClientBindingModel model);
        void Insert(ClientBindingModel model);
        void Delete(ClientBindingModel model);
    }
}
