using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.ViewModels;
using System.Linq;

namespace GadgetStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public HomeController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var featuredProducts = _context.Products.Take(4).ToList();
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = featuredProducts
            };
            return View(viewModel);
        }
    }
}
