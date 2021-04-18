using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class Storage
    {
        public int Id { get; set; }
        [Required]
        public string StorageName { get; set; }
        [Required]
        public string StorageManager { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        [ForeignKey("StorageId")]
        public List<StorageBlank> StorageBlanks { get; set; }
    }
}
