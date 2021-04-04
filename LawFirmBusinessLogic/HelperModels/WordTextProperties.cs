using DocumentFormat.OpenXml.Wordprocessing;

namespace LawFirmBusinessLogic.HelperModels
{
    public class WordTextProperties
    {
        public bool Bold { get; internal set; }
        public string Size { get; internal set; }
        public JustificationValues JustificationValues { get; internal set; }
    }
}