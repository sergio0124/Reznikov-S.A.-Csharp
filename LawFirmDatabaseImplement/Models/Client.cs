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
        public string ClientName { get; set; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Email { set; get; }
        public int OrderId { set; get; }
    }
}
