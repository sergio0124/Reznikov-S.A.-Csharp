using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmDatabaseImplement.Implements
{
    public class ImplementerStorage: IImplementerStorage
    {
        public List<ImplementerViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Implementers
                .Select(rec => new ImplementerViewModel
                {
                    Id = rec.Id,
                    ImplementerFIO = rec.ImplementerFIO
                })
               .ToList();
            }
        }
        public List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                return context.Implementers
                .Where(rec => rec.ImplementerFIO.Contains(model.ImplementerFIO))
               .Select(rec => new ImplementerViewModel
               {
                   Id = rec.Id,
                   ImplementerFIO = rec.ImplementerFIO
               })
                .ToList();
            }
        }
        public ImplementerViewModel GetElement(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                var implementer = context.Implementers
                .FirstOrDefault(rec => rec.ImplementerFIO == model.ImplementerFIO ||
               rec.Id == model.Id);
                return implementer != null ?
                new ImplementerViewModel
                {
                    Id = implementer.Id,
                    ImplementerFIO = implementer.ImplementerFIO
                } :
               null;
            }
        }
        public void Insert(ImplementerBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                context.Implementers.Add(CreateModel(model, new Implementer()));
                context.SaveChanges();
            }
        }
        public void Update(ImplementerBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                var element = context.Implementers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(ImplementerBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Implementer element = context.Implementers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Implementers.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Implementer CreateModel(ImplementerBindingModel model, Implementer implementer)
        {
            implementer.ImplementerFIO = model.ImplementerFIO;
            return implementer;
        }
    }
}
