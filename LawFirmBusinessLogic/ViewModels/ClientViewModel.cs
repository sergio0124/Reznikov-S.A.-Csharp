using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [DataMember]
        public int Id { set; get; }
        [DisplayName("ClientFIO")]
        [DataMember]
        public string ClientFIO { get; set; }
        [DataMember]
        [DisplayName("Email")]
        public string Email { get; set; }
        [DataMember]
        [DisplayName("Password")]
        public string Password { set; get; }
    }
}
