using FUS.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUS.Core.Entities
{
    [Table("Files")]

    public class File
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("FileType")]
        public FileTypeEnum Type { get; set; }
        [Column("FilePath")]
        public string FilePath { get; set; }
        [Column("CustomerId")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
