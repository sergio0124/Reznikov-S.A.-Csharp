using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LawFirmBusinessLogic.ViewModels
{
    public class BlankViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название компонента")]
        public string BlankName { get; set; }
    }
}
