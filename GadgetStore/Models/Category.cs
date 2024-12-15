using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GadgetStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        // Navigation property
        public ICollection<Product>? Products { get; set; }
    }
}
