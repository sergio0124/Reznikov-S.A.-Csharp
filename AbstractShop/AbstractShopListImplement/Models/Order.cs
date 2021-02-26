using AbstractShopBusinessLogic.Enums;
using System;
namespace AbstractShopListImplement.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public String ProductName { set; get; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
    }
}