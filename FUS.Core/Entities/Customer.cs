using System.ComponentModel.DataAnnotations.Schema;

namespace FUS.Core.Entities
{
    [Table("Customer")]
    public class Customer
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
    }
}
