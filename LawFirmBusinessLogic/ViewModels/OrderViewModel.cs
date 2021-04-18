using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using LawFirmBusinessLogic.Enums;
using System.Runtime.Serialization;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class OrderViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int DocumentId { get; set; }
        [DataMember]
        public int? ImplementerId { get; set; }
        [DataMember]
        [DisplayName("Клиент")]
        public string ClientFIO { get; set; }
        [DataMember]
        [DisplayName("Исполнитель")]
        public string ImplementerFIO { get; set; }
        [DataMember]
        [DisplayName("Изделие")]
        public string DocumentName { get; set; }
        [DataMember]
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DataMember]
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        [DataMember]
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }
        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        [DataMember]
        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }

}
