using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmDatabaseImplement.Implements
{
    public class ClientStorage : IClientStorage
    {
        public void Delete(ClientBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                var client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);

                if (client == null)
                {
                    throw new Exception("Клиент не найден");
                }

                context.Clients.Remove(client);
                context.SaveChanges();
            }
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new LawFirmDatabase())
            {
                var client = context.Clients
                    .FirstOrDefault(rec => rec.Id == model.Id);

                return client != null ?
                    new ClientViewModel
                    {
                        Id = client.Id,
                        ClientName = client.ClientName,
                        Email = client.Email
                    } :
                    null;
            }
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new LawFirmDatabase())
            {
                return context.Clients
                    .Where(rec => rec.ClientName.Contains(model.ClientName))
                    .ToList()
                    .Select(rec => new ClientViewModel
                    {
                        Id = rec.Id,
                        ClientName = rec.ClientName,
                        Email = rec.Email
                    })
                    .ToList();
            }
        }

        public List<ClientViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Clients
                    .Select(rec => new ClientViewModel
                    {
                        Id = rec.Id,
                        ClientName = rec.ClientName,
                        Email = rec.Email
                    })
                    .ToList();
            }
        }

        public void Insert(ClientBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Client());
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

        public void Update(ClientBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
                        if (client == null)
                        {
                            throw new Exception("клиент не найден");
                        }
                        CreateModel(model, client);
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

        private Client CreateModel(ClientBindingModel model, Client client)
        {
            client.ClientName = model.ClientName;
            client.Password = model.Password;
            client.Email = model.Email;
            client.Id = (int)model.Id;           
            return client;
        }
    }
}
