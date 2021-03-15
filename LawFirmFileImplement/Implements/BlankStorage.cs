using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmFileImplement.Implements
{
    public class BlankStorage : IBlankStorage
    {
        private readonly FileDataListSingleton source;

 public BlankStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public List<BlankViewModel> GetFullList()
        {
            return source.Blanks
            .Select(CreateModel)
           .ToList();
        }
        public List<BlankViewModel> GetFilteredList(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Blanks
            .Where(rec => rec.BlankName.Contains(model.BlankName))
           .Select(CreateModel)
            .ToList();
        }
        public BlankViewModel GetElement(BlankBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var blanks = source.Blanks
            .FirstOrDefault(rec => rec.BlankName == model.BlankName ||
           rec.Id == model.Id);
            return blanks != null ? CreateModel(blanks) : null;
        }
        public void Insert(BlankBindingModel model)
        {
            int maxId = source.Blanks.Count > 0 ? source.Blanks.Max(rec =>
           rec.Id) : 0;
            var element = new Blank { Id = maxId + 1 };
            source.Blanks.Add(CreateModel(model, element));
        }
        public void Update(BlankBindingModel model)
        {
            var element = source.Blanks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
        }
        public void Delete(BlankBindingModel model)
        {
            Blank element = source.Blanks.FirstOrDefault(rec => rec.Id ==
           model.Id);
            if (element != null)
            {
                
            source.Blanks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
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
