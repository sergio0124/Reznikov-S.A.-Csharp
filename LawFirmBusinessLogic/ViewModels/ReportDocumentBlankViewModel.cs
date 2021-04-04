using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    public class ReportDocumentBlankViewModel
    {
        public string BlankName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Documents { get; set; }
    }
}
