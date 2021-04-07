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
        [DisplayName("Имя пользователя")]
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
