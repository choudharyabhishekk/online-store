using GadgetStore.Models;
using System.Collections.Generic;

namespace GadgetStore.ViewModels
{
    public class ProductsViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
