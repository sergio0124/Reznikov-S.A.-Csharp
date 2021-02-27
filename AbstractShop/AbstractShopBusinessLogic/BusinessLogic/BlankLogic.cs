using System;
using System.Collections.Generic;
using System.Text;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;


namespace LawFirmBusinessLogic.BusinessLogic
{
    public class BlankLogic
    {
        private readonly IBlankStorage _blankStorage;
        public BlankLogic(IBlankStorage blankStorage)
        {
            _blankStorage = blankStorage;
        }
        public List<BlankViewModel> Read(BlankBindingModel model)
        {
            if (model == null)
            {
                return _blankStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<BlankViewModel> { _blankStorage.GetElement(model)
};
            }
            return _blankStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(BlankBindingModel model)
        {
            var element = _blankStorage.GetElement(new BlankBindingModel
            {
                BlankName = model.BlankName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть бланк с таким названием");
            }
            if (model.Id.HasValue)
            {
                _blankStorage.Update(model);
            }
            else
            {
                _blankStorage.Insert(model);
            }
        }
        public void Delete(BlankBindingModel model)
        {
            var element = _blankStorage.GetElement(new BlankBindingModel
            {
                Id =
           model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _blankStorage.Delete(model);
        }
    }
}
