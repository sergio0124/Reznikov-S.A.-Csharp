using AbstractShopListImplement.Models;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawFirmListImplement.Implements
{
    public class StorageStorage: IStorageStorage
    {
        private readonly DataListSingleton source;

        public StorageStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StorageViewModel> GetFullList()
        {
            List<StorageViewModel> result = new List<StorageViewModel>();

            foreach (var storage in source.Storages)
            {
                result.Add(CreateModel(storage));
            }

            return result;
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            List<StorageViewModel> result = new List<StorageViewModel>();

            foreach (var storage in source.Storages)
            {
                if (storage.StorageName.Contains(model.StorageName))
                {
                    result.Add(CreateModel(storage));
                }
            }

            return result;
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var storage in source.Storages)
            {
                if (storage.Id == model.Id || storage.StorageName == model.StorageName)
                {
                    return CreateModel(storage);
                }
            }

            return null;
        }

        public void Insert(StorageBindingModel model)
        {
            Storage tempStorage = new Storage
            {
                Id = 1,
                StorageBlanks = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };

            foreach (var storage in source.Storages)
            {
                if (storage.Id >= tempStorage.Id)
                {
                    tempStorage.Id = storage.Id + 1;
                }
            }

            source.Storages.Add(CreateModel(model, tempStorage));
        }

        public void Update(StorageBindingModel model)
        {
            Storage tempStorage = null;

            foreach (var storage in source.Storages)
            {
                if (storage.Id == model.Id)
                {
                    tempStorage = storage;
                }
            }

            if (tempStorage == null)
            {
                throw new Exception("Элемент не найден");
            }

            CreateModel(model, tempStorage);
        }

        public void Delete(StorageBindingModel model)
        {
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id == model.Id)
                {
                    source.Storages.RemoveAt(i);

                    return;
                }
            }
            throw new Exception("Элемент не найден");
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
    }
}
