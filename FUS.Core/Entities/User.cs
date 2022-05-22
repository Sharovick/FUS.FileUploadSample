using System.ComponentModel.DataAnnotations.Schema;

namespace FUS.Core.Entities
{
    [Table("User")]
    public class User
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("LoginName")]
        public string LoginName { get; set; }
        [Column("Ip")]
        public string Ip { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
    }
}
