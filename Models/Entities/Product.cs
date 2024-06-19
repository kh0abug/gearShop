using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DisplayName("Price")]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        [DisplayName("Stock")]
        [Range(1, 9999)]
        public int Stock { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [DisplayName("Product Image")]
        [ValidateNever]
        public string ImgUrl { get; set; }

        [ValidateNever]
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
