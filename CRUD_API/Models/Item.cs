using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    public class Item
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
            
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }  
        public Category Category { get; set; }

        //public IFormFile? image { get; set; }


    }
}
