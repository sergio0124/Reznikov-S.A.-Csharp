using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class DocumentBlank
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int BlankId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Blank Blank { get; set; }
        public virtual Document Document { get; set; }

    }
}
