using System;
using System.Collections.Generic;
using System.Linq;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmDatabaseImplement.Implements

{
    public class StorageStorage : IStorageStorage
    {
        public List<StorageViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Storages
                    .Include(rec => rec.StorageBlanks)
                    .ThenInclude(rec => rec.Blank)
                    .ToList().Select(rec => new StorageViewModel
                    {
                        Id = rec.Id,
                        StorageName = rec.StorageName,
                        StorageManager = rec.StorageManager,
                        DateCreate = rec.DateCreate,
                        StorageBlanks = rec.StorageBlanks
                            .ToDictionary(recPPC => recPPC.BlankId,
                            recPPC => (recPPC.Blank?.BlankName, recPPC.Count))
                    })
                    .ToList();
            }
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new LawFirmDatabase())
            {
                return context.Storages
                    .Include(rec => rec.StorageBlanks)
                    .ThenInclude(rec => rec.Blank)
                    .Where(rec => rec.StorageName
                    .Contains(model.StorageName))
                    .ToList()
                    .Select(rec => new StorageViewModel
                    {
                        Id = rec.Id,
                        StorageName = rec.StorageName,
                        StorageManager = rec.StorageManager,
                        DateCreate = rec.DateCreate,
                        StorageBlanks = rec.StorageBlanks
                            .ToDictionary(recPC => recPC.BlankId, recPC => (recPC.Blank?.BlankName, recPC.Count))
                    })
                    .ToList();
            }
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new LawFirmDatabase())
            {
                var storehouse = context.Storages
                    .Include(rec => rec.StorageBlanks)
                    .ThenInclude(rec => rec.Blank)
                    .FirstOrDefault(rec => rec.StorageName == model.StorageName || rec.Id == model.Id);

                return storehouse != null ?
                    new StorageViewModel
                    {
                        Id = storehouse.Id,
                        StorageName = storehouse.StorageName,
                        StorageManager = storehouse.StorageManager,
                        DateCreate = storehouse.DateCreate,
                        StorageBlanks = storehouse.StorageBlanks
                            .ToDictionary(recPC => recPC.BlankId, recPC => (recPC.Blank?.BlankName, recPC.Count))
                    } :
                    null;
            }
        }

        public void Insert(StorageBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Storage(), context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(StorageBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Storages.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(StorageBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Storage element = context.Storages.FirstOrDefault(rec => rec.Id == model.Id);

                if (element != null)
                {
                    context.Storages.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Storage CreateModel(StorageBindingModel model, Storage storage, LawFirmDatabase context)
        {
            storage.StorageName = model.StorageName;
            storage.StorageManager = model.StorageManager;
            storage.DateCreate = model.DateCreate;

            if (storage.Id == 0)
            {
                context.Storages.Add(storage);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var storageBlanks = context.StorageBlanks
                    .Where(rec => rec.StorageId == model.Id.Value)
                    .ToList();

                context.StorageBlanks
                    .RemoveRange(storageBlanks
                        .Where(rec => !model.StorageBlanks
                            .ContainsKey(rec.BlankId))
                                .ToList());
                context.SaveChanges();

                foreach (var updateBlank in storageBlanks)
                {
                    updateBlank.Count = model.StorageBlanks[updateBlank.BlankId].Item2;
                    model.StorageBlanks.Remove(updateBlank.BlankId);
                }
                context.SaveChanges();
            }

            foreach (var blank in model.StorageBlanks)
            {
                context.StorageBlanks.Add(new StorageBlank
                {
                    StorageId = storage.Id,
                    BlankId = blank.Key,
                    Count = blank.Value.Item2
                });

                context.SaveChanges();
            }

            return storage;
        }

        public void CheckBlanks(DocumentViewModel model, int blankCountInOrder)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {

                    foreach (var blanksInDocument in model.DocumentBlanks)
                    {
                        int blanksCountInDocument = blanksInDocument.Value.Item2 * blankCountInOrder;

                        List<StorageBlank> oneOfBlank = context.StorageBlanks
                            .Where(storehouse => storehouse.BlankId == blanksInDocument.Key)
                            .ToList();

                        foreach (var blank in oneOfBlank)
                        {
                            int blankCountInStorage = blank.Count;

                            if (blankCountInStorage <= blanksCountInDocument)
                            {
                                blanksCountInDocument -= blankCountInStorage;
                                context.Storages.FirstOrDefault(rec => rec.Id == blank.StorageId).StorageBlanks.Remove(blank);
                            }
                            else
                            {
                                blank.Count -= blanksCountInDocument;
                                blanksCountInDocument = 0;
                            }

                            if (blanksCountInDocument == 0)
                            {
                                break;
                            }
                        }

                        if (blanksCountInDocument > 0)
                        {
                            transaction.Rollback();

                            throw new Exception("Не хватает компонентов для изготовления данного печатного изделия!");
                        }
                    }

                    context.SaveChanges();

                    transaction.Commit();
                }
            }
        }
    }
}