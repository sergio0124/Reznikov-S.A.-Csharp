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
    public class BlankStorage:IBlankStorage
    {
        public List<BlankViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Blanks
                .Select(rec => new BlankViewModel
                {
                    Id = rec.Id,
                    BlankName = rec.BlankName
                })
               .ToList();
            }
        }
        public List<BlankViewModel> GetFilteredList(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                return context.Blanks
                .Where(rec => rec.BlankName.Contains(model.BlankName))
               .Select(rec => new BlankViewModel
               {
                   Id = rec.Id,
                   BlankName = rec.BlankName
               })
                .ToList();
            }
        }
        public BlankViewModel GetElement(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                var blank = context.Blanks
                .FirstOrDefault(rec => rec.BlankName == model.BlankName ||
               rec.Id == model.Id);
                return blank != null ?
                new BlankViewModel
                {
                    Id = blank.Id,
                    BlankName = blank.BlankName
                } :
               null;
            }
        }
        public void Insert(BlankBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                context.Blanks.Add(CreateModel(model, new Blank()));
                context.SaveChanges();
            }
        }
        public void Update(BlankBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                var element = context.Blanks.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null) 
            {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(BlankBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Blank element = context.Blanks.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Blanks.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Blank CreateModel(BlankBindingModel model, Blank blank)
        {
            blank.BlankName = model.BlankName;
            return blank;
        }

    }
}
