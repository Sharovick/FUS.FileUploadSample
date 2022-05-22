using System.ComponentModel.DataAnnotations.Schema;

namespace FUS.Core.Entities
{
    [Table("FileType")]
    public class FileType
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
