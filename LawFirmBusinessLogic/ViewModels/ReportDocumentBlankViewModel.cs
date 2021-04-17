using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class ReportDocumentBlankViewModel
    {
        [DataMember]
        public string BlankName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<Tuple<string, int>> Documents { get; set; }
    }
}
