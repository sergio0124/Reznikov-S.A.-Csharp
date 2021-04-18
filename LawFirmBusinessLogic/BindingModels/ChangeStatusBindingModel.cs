using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    public class ChangeStatusBindingModel
    {
        public int OrderId { get; set; }
        public int? ImplementerId { set; get; }
    }
}
