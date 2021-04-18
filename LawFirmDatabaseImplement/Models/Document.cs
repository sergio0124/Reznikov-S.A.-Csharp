﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class Document
    {
        public int Id { set; get; }
        [Required]
        public string DocumentName { get; set; }
        [Required]
        public decimal Price { set; get; }
        [ForeignKey("DocumentId")]
        public List<DocumentBlank> DocumentBlanks { get; set; }
        [ForeignKey("DocumentId")]
        public List<Order> Order { get; set; }
    }
}
