using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBookCoreMVC.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage="Display method must be between 1 and 100 only")]
        public int DisplayOrder { get; set; }

        public DateTime DateTimeCreated { get; set; } = DateTime.Now;





    }
}
