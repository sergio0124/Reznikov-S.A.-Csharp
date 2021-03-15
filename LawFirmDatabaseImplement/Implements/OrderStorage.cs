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
    public class OrderStorage : IOrderStorage
    {
        public void Delete(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                var order = context.Orders
               .FirstOrDefault(rec => rec.Id
               == model.Id);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    Count = order.Count,
                    Status=order.Status,
                    DateCreate=order.DateCreate,
                    DateImplement=order.DateImplement,
                    Sum=order.Sum,
                    DocumentName=order.DocumentName,
                    DocumentId=order.DocumentId                   
                } :
               null;
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                return context.Orders
               .Where(rec => rec.DocumentName.Contains(model.DocumentName))
               .ToList()
               .Select(rec => new OrderViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.DocumentName,
                   Count = rec.Count,
                   DocumentId = rec.DocumentId,
                   DateImplement=rec.DateImplement,
                   DateCreate=rec.DateCreate,
                   Status=rec.Status,
                   Sum=rec.Sum
               })
               .ToList();
            }
        }

        public List<OrderViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Orders
               .ToList()
               .Select(rec => new OrderViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.DocumentName,
                   Count = rec.Count,
                   DocumentId = rec.DocumentId,
                   DateImplement = rec.DateImplement,
                   DateCreate = rec.DateCreate,
                   Status = rec.Status,
                   Sum = rec.Sum
               })
               .ToList();
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Orders.Add(CreateModel(model, new Order(), context));
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

        private Order CreateModel(OrderBindingModel model, Order order, LawFirmDatabase context)
        {
            order.DocumentId = model.DocumentId;
            int money = 0;
            Document document = context.Documents.FirstOrDefault(rec => rec.Id == order.DocumentId);
            money = (int)document.Price * model.Count;
            order.Sum = money;
            order.Count = model.Count;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
            }
        }
    }
}
