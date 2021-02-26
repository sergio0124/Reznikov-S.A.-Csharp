using System;
using System.Collections.Generic;
using System.Text;
using AbstractShopBusinessLogic.BindingModels;
using AbstractShopBusinessLogic.ViewModels;

namespace AbstractShopBusinessLogic.Interfaces
{
    public interface IProductStorage
    {
        List<ProductViewModel> GetFullList();
        List<ProductViewModel> GetFilteredList(ProductBindingModel model);
        ProductViewModel GetElement(ProductBindingModel model);
        void Insert(ProductBindingModel model);
        void Update(ProductBindingModel model);
        void Delete(ProductBindingModel model);
    }
}
