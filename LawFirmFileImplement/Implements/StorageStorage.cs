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
    public class StorageStorage: IStorageStorage
    {
        private readonly FileDataListSingleton source;

        public StorageStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public List<StorageViewModel> GetFullList()
        {
            return source.Storages
            .Select(CreateModel)
           .ToList();
        }
        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Storages
            .Where(rec => rec.StorageName.Contains(model.StorageName))
           .Select(CreateModel)
            .ToList();
        }
        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var storages = source.Storages
            .FirstOrDefault(rec => rec.StorageName == model.StorageName ||
           rec.Id == model.Id);
            return storages != null ? CreateModel(storages) : null;
        }
        public void Insert(StorageBindingModel model)
        {
            int maxId = source.Storages.Count > 0 ? source.Storages.Max(xStorage => xStorage.Id) : 0;
            var storage = new Storage { Id = maxId + 1, StorageBlanks = new Dictionary<int, int>(), DateCreate = DateTime.Now };
            source.Storages.Add(CreateModel(model, storage));
        }

        public void Update(StorageBindingModel model)
        {
            var storage = source.Storages.FirstOrDefault(XStorage => XStorage.Id == model.Id);

            if (storage == null)
            {
                throw new Exception("Склад не найден");
            }

            CreateModel(model, storage);
        }
        public void Delete(StorageBindingModel model)
        {
            Storage element = source.Storages.FirstOrDefault(rec => rec.Id ==
           model.Id);
            if (element != null)
            {
                source.Storages.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private Storage CreateModel(StorageBindingModel model, Storage storage)
        {
            storage.StorageName = model.StorageName;
            storage.StorageManager = model.StorageManager;
            foreach (var key in storage.StorageBlanks.Keys.ToList())
            {
                if (!model.StorageBlanks.ContainsKey(key))
                {
                    storage.StorageBlanks.Remove(key);
                }
            }

            foreach (var blanks in model.StorageBlanks)
            {
                if (storage.StorageBlanks.ContainsKey(blanks.Key))
                {
                    storage.StorageBlanks[blanks.Key] = model.StorageBlanks[blanks.Key].Item2;
                }
                else
                {
                    storage.StorageBlanks.Add(blanks.Key, model.StorageBlanks[blanks.Key].Item2);
                }
            }
            return storage;
        }
        private StorageViewModel CreateModel(Storage storage)
        {
            Dictionary<int, (string, int)> StorageBlanks = new Dictionary<int, (string, int)>();

            foreach (var storageBlank in storage.StorageBlanks)
            {
                string blankName = string.Empty;

                foreach (var blank in source.Blanks)
                {
                    if (storageBlank.Key == blank.Id)
                    {
                        blankName = blank.BlankName;

                        break;
                    }
                }
                StorageBlanks.Add(storageBlank.Key, (blankName, storageBlank.Value));
            }

            return new StorageViewModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                StorageManager = storage.StorageManager,
                DateCreate = storage.DateCreate,
                StorageBlanks = StorageBlanks
            };
        }

        public bool TakeFromStorage(Dictionary<int, (string, int)> blanks, int count)
        {
            foreach (var blank in blanks)
            {
                int _count = source.Storages.
                    Where(document => document.StorageBlanks
                    .ContainsKey(blank.Key))
                    .Sum(document => document.StorageBlanks[blank.Key]);

                if (_count < blank.Value.Item2 * count)
                {
                    return false;
                }
            }

            foreach (var blank in blanks)
            {
                int number = blank.Value.Item2 * count;
                IEnumerable<Storage> storages = source.Storages.Where(document => document.StorageBlanks.ContainsKey(blank.Key));

                foreach (Storage storage in storages)
                {
                    if (storage.StorageBlanks[blank.Key] <= number)
                    {
                        number -= storage.StorageBlanks[blank.Key];
                        storage.StorageBlanks.Remove(blank.Key);
                    }
                    else
                    {
                        storage.StorageBlanks[blank.Key] -= number;
                        number = 0;
                    }

                    if (number == 0)
                    {
                        break;
                    }
                }
            }

            return true;
        }
    }
}
