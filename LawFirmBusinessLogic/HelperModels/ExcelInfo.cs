using LawFirmBusinessLogic.ViewModels;
using System.Collections.Generic;


namespace LawFirmBusinessLogic.HelperModels
{
    public class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportDocumentBlankViewModel> DocumentBlanks { get; set; }

    }
}
