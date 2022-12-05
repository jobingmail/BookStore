using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBookCoreMVC.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ISBN { get; set; }
        [Required]
        public string? Author { get; set; }

        public string? ImageURL { get; set; }

        //[Required(ErrorMessage = "The {0} field is required")]
        //[Display(Name = "Image")]
        //[DataType(DataType.Upload)]
        //public IFormFile Image { get; set; }


        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        

        // Foreign keys 
        
        //[Display(Name = "Category")]

        public int CategoryId { get; set; }
        public Category? Categories { get; set; }


       
        [Required]
        [Display(Name = "Cover Type")]
   
        public int CoverTypeId { get; set; }
        public CoverType? CoverType { get; set; }




        [NotMapped]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? CoverTypeName { get; set; }

    }
}
