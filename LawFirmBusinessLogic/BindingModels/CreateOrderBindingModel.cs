using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    [DataContract]
    public class CreateOrderBindingModel
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int DocumentId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }

}
