using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class Client
    {
        public int Id { set; get; }
        [Required]
        public string ClientFIO { get; set; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Email { set; get; }
        public virtual List<Order> Orders { set; get; }
    }
}
