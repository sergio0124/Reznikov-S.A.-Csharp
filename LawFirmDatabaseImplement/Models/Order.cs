using LawFirmBusinessLogic.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LawFirmDatabaseImplement.Models
{
    public class Order
    {
        public int Id { set; get; }
        public String DocumentName { set; get; }
        public int DocumentId { set; get; }
        [Required]
        public int Count { set; get; }
        [Required]
        public int Sum { set; get; }
        [Required]
        public OrderStatus Status { set; get; }
        [Required]
        public DateTime DateCreate { set; get; }
        public DateTime? DateImplement { get; set; }
    }
}