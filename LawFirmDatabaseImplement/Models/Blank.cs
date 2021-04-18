﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class Blank
    {
        public int Id { get; set; }
        [Required]
        public string BlankName { get; set; }
        [ForeignKey("BlankId")]
        public List<DocumentBlank> DocumentBlanks { get; set; }
        [ForeignKey("BlankId")]
        public List<StorageBlank> StorageBlanks { get; set; }

    }
}
