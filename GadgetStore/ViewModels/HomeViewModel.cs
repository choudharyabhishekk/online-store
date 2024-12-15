using GadgetStore.Models;
using System.Collections.Generic;

namespace GadgetStore.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FeaturedProducts { get; set; }
    }
}
