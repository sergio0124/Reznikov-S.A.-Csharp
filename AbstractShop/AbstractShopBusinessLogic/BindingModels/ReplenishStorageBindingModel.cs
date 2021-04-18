using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.BindingModels
{
    public class ReplenishStorageBindingModel
    {
        public int StorageId { get; set; }
        public int BlankId { get; set; }
        public int Count { get; set; }
    }
}
