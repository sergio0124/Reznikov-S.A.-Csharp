using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawFirmListImplement.Implements
{
    public class ImplementerStorage: IImplementerStorage
    {
        private readonly DataListSingleton source;
        public ImplementerStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ImplementerViewModel> GetFullList()
        {
            List<ImplementerViewModel> result = new List<ImplementerViewModel>();
            foreach (var component in source.Implementers)
            {
                result.Add(CreateModel(component));
            }
            return result;
        }
        public List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<ImplementerViewModel> result = new List<ImplementerViewModel>();
            foreach (var component in source.Implementers)
            {
                if (component.ImplementerFIO.Contains(model.ImplementerFIO))
                {
                    result.Add(CreateModel(component));
                }
            }
            return result;
        }
        public ImplementerViewModel GetElement(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var component in source.Implementers)
            {
                if (component.Id == model.Id || component.ImplementerFIO ==
               model.ImplementerFIO)
                {
                    return CreateModel(component);
                }
            }
            return null;
        }
        public void Insert(ImplementerBindingModel model)
        {
            Implementer tempComponent = new Implementer { Id = 1 };
            foreach (var component in source.Implementers)
            {
                if (component.Id >= tempComponent.Id)
                {
                    tempComponent.Id = component.Id + 1;
                }
            }
            source.Implementers.Add(CreateModel(model, tempComponent));
        }
        public void Update(ImplementerBindingModel model)
        {

            Implementer tempImplementer = null;
            foreach (var component in source.Implementers)
            {
                if (component.Id == model.Id)
                {
                    tempImplementer = component;
                }
            }
            if (tempImplementer == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempImplementer);
        }
        public void Delete(ImplementerBindingModel model)
        {
            for (int i = 0; i < source.Implementers.Count; ++i)
            {
                if (source.Implementers[i].Id == model.Id)
                {
                    source.Implementers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Implementer CreateModel(ImplementerBindingModel model, Implementer implementer)
        {
            implementer.ImplementerFIO = model.ImplementerFIO;
            implementer.PauseTime = model.PauseTime;
            implementer.WorkingTime = model.WorkingTime;
            return implementer;
        }
        private ImplementerViewModel CreateModel(Implementer implementer)
        {
            return new ImplementerViewModel
            {
                Id = implementer.Id,
                ImplementerFIO = implementer.ImplementerFIO,
                WorkingTime=implementer.WorkingTime,
                PauseTime=implementer.PauseTime
            };
        }
    }
}
