using System.ComponentModel.DataAnnotations;

namespace BulkyBookCoreMVC.Models
{
    public class ErrorLog
    {
        [Key]
        public Guid Guid { get; set; }= Guid.NewGuid();

        [Required]
        public DateTime CreatedDate { get; set; }= DateTime.Now;

        [Required]
        [StringLength(2000)]
        public string? ErrorMessasge { get; set; }
        

    }
}
