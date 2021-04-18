using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class StorageLogic
    {
        private readonly IStorageStorage _storageStorage;

        private readonly IBlankStorage _blankStorage;

        public StorageLogic(IStorageStorage storage, IBlankStorage blankStorage)
        {
            _storageStorage = storage;
            _blankStorage = blankStorage;
        }

        public List<StorageViewModel> Read(StorageBindingModel model)
        {
            if (model == null)
            {
                return _storageStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<StorageViewModel>
                {
                    _storageStorage.GetElement(model)
                };
            }

            return _storageStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(StorageBindingModel model)
        {
            var element = _storageStorage.GetElement(new StorageBindingModel
            {
                StorageName = model.StorageName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть склад с таким названием");
            }

            if (model.Id.HasValue)
            {
                _storageStorage.Update(model);
            }
            else
            {
                _storageStorage.Insert(model);
            }
        }

        public void Delete(StorageBindingModel model)
        {
            var element = _storageStorage.GetElement(new StorageBindingModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            _storageStorage.Delete(model);
        }

        public void Replenishment(ReplenishStorageBindingModel model)
        {
            var storage = _storageStorage.GetElement(new StorageBindingModel
            {
                Id = model.StorageId
            });

            var blank = _blankStorage.GetElement(new BlankBindingModel
            {
                Id = model.BlankId
            });

            if (storage == null)
            {
                throw new Exception("Не найден склад");
            }

            if (blank == null)
            {
                throw new Exception("Не найден материал");
            }

            if (storage.StorageBlanks.ContainsKey(model.BlankId))
            {
                storage.StorageBlanks[model.BlankId] = (blank.BlankName, storage.StorageBlanks[model.BlankId].Item2 + model.Count);
            }
            else
            {
                storage.StorageBlanks.Add(blank.Id, (blank.BlankName, model.Count));
            }

            _storageStorage.Update(new StorageBindingModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                StorageManager = storage.StorageManager,
                DateCreate = storage.DateCreate,
                StorageBlanks = storage.StorageBlanks
            });
        }
    }
}
