using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    [DataContract]
    public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string ClientFIO { get; set; }
    }
}
