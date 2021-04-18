using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
namespace LawFirmListImplement.Implements
{
    public class BlankStorage : IBlankStorage
    {
        private readonly DataListSingleton source;
        public BlankStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BlankViewModel> GetFullList()
        {
            List<BlankViewModel> result = new List<BlankViewModel>();
            foreach (var component in source.Blanks)
            {
                result.Add(CreateModel(component));
            }
            return result;
        }
        public List<BlankViewModel> GetFilteredList(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<BlankViewModel> result = new List<BlankViewModel>();
            foreach (var component in source.Blanks)
            {
                if (component.BlankName.Contains(model.BlankName))
                {
                    result.Add(CreateModel(component));
                }
            }
            return result;
        }
        public BlankViewModel GetElement(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var component in source.Blanks)
            {
                if (component.Id == model.Id || component.BlankName ==
               model.BlankName)
                {
                    return CreateModel(component);
                }
            }
            return null;
        }
        public void Insert(BlankBindingModel model)
        {
            Blank tempComponent = new Blank { Id = 1 };
            foreach (var component in source.Blanks)
            {
                if (component.Id >= tempComponent.Id)
                {
                    tempComponent.Id = component.Id + 1;
                }
            }
            source.Blanks.Add(CreateModel(model, tempComponent));
        }
        public void Update(BlankBindingModel model)
        {

            Blank tempBlank = null;
            foreach (var component in source.Blanks)
            {
                if (component.Id == model.Id)
                {
                    tempBlank = component;
                }
            }
            if (tempBlank == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempBlank);
        }
        public void Delete(BlankBindingModel model)
        {
            for (int i = 0; i < source.Blanks.Count; ++i)
            {
                if (source.Blanks[i].Id == model.Id.Value)
                {
                    source.Blanks.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Blank CreateModel(BlankBindingModel model, Blank blank)
        {
            blank.BlankName = model.BlankName;
            return blank;
        }
        private BlankViewModel CreateModel(Blank blank)
        {
            return new BlankViewModel
            {
                Id = blank.Id,
                BlankName = blank.BlankName
            };
        }
    }
}
