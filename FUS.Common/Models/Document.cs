using FUS.Common.Enums;

namespace FUS.Common.Models
{
    public class Document
    {
        public FileTypeEnum Type { get; set; }
        public string FilePath { get; set; }
    }
}
