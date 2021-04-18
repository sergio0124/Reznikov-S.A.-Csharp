﻿using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Enums;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmFileImplement.Implements
{
    public class OrderStorage: IOrderStorage
    {
		private readonly FileDataListSingleton source;


		public OrderStorage()
		{
			source = FileDataListSingleton.GetInstance();
		}


		public List<OrderViewModel> GetFullList()
		{
			List<OrderViewModel> result = new List<OrderViewModel>();
			foreach (var order in source.Orders)
			{
				result.Add(CreateModel(order));
			}
			return result;
		}


		public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			return source.Orders
			.Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
		   rec.DateCreate.Date == model.DateCreate.Date) ||
			(model.DateFrom.HasValue && model.DateTo.HasValue && rec.DateCreate.Date
		   >= model.DateFrom.Value.Date && rec.DateCreate.Date <= model.DateTo.Value.Date) ||
			(model.ClientId.HasValue && rec.ClientId == model.ClientId)
			 || (model.Status.HasValue && model.Status == rec.Status && rec.Status == OrderStatus.Выполняется && model.ImplemeterId.HasValue && rec.ImplementerId == rec.ImplementerId)
					|| (model.Status.HasValue && model.Status == rec.Status && rec.Status == OrderStatus.Принят)) 
			.Select(CreateModel)
			.ToList();
 }



		public OrderViewModel GetElement(OrderBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			foreach (var order in source.Orders)
			{
				if (order.Id == model.Id || order.DocumentId ==
			   model.DocumentId)
				{
					return CreateModel(order);
				}
			}
			return null;
		}


		public void Insert(OrderBindingModel model)
		{
			Order tempOrder = new Order
			{
				Id = 1
			};
			foreach (var order in source.Orders)
			{
				if (order.Id >= tempOrder.Id)
				{
					tempOrder.Id = order.Id + 1;
				}
			}
			source.Orders.Add(CreateModel(model, tempOrder));
		}


		public void Update(OrderBindingModel model)
		{
			Order tempOrder = null;
			foreach (var order in source.Orders)
			{
				if (order.Id == model.Id)
				{
					tempOrder = order;
				}
			}
			if (tempOrder == null)
			{
				throw new Exception("Элемент не найден");
			}
			CreateModel(model, tempOrder);
		}


		public void Delete(OrderBindingModel model)
		{
			for (int i = 0; i < source.Orders.Count; ++i)
			{
				if (source.Orders[i].Id == model.Id)
				{
					source.Orders.RemoveAt(i);
					return;
				}
			}
			throw new Exception("Элемент не найден");
		}


		private Order CreateModel(OrderBindingModel model, Order order)
		{
			order.DocumentId = model.DocumentId;
			order.Sum = model.Sum;
			order.Count = model.Count;
			order.Status = (OrderStatus)model.Status;
			order.DateCreate = model.DateCreate;
			order.DateImplement = model.DateImplement;
			return order;
		}


		private OrderViewModel CreateModel(Order order)
		{
			return new OrderViewModel
			{
				Id = order.Id,
				DocumentId = order.DocumentId,
				Count = order.Count,
				Sum = order.Sum,
				Status = order.Status,
				DateCreate = order.DateCreate,
				DateImplement = order.DateImplement
			};
		}
	}
}
