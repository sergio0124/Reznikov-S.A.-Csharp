using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    public class DocumentBindingModel
    {
        public int? Id { get; set; }
        public string DocumentName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> DocumentBlanks { get; set; }
    }
}
