using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class StorageBlank
    {
        public int Id { get; set; }
        public int StorageId { get; set; }
        public int BlankId { get; set; }
        [Required]
        public int Count { get; set; }
        public Blank Blank { get; set; }
        public Document Document { get; set; }
    }
}
