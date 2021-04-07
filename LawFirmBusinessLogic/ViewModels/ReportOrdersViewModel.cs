using LawFirmBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    public class ReportOrdersViewModel
    {
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        [DisplayName("Имя документа")]
        public string DocumentName { get; set; }
        [DisplayName("Имя клиента")]
        public string ClientName { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Цена")]
        public decimal Sum { get; set; }
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }
    }
}
