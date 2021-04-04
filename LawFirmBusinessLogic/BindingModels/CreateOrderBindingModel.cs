using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    public class CreateOrderBindingModel
    {
        public int DocumentId { get; set; }
        public decimal Sum { set; get; }
        public int Count { get; set; }
    }
}
