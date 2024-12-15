using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GadgetStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public ProductsController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        [Route("/products")]
        public IActionResult Index(int? categoryId)
        {
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();

            var productsQuery = _context.Products.AsQueryable();

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = productsQuery.ToList();

            var viewModel = new ProductsViewModel
            {
                Products = products,
                Categories = categories,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }

        [Route("/products/{id}")]
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
