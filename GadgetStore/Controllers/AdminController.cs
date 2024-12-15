using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GadgetStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public AdminController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        [HttpGet("")]
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        // Categories Management
        [HttpGet("Categories")]
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View("Categories/Index", categories); 
        }

        // Create Category
        [HttpGet("Categories/Create")]
        public IActionResult CreateCategory()
        {
            return View("Categories/Create"); 
        }

        [HttpPost("Categories/Create")]
        public IActionResult CreateCategory(Category category)
        {
            Console.WriteLine("CreateCategory POST method called.");

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Adding category: {category.Name}");
                _context.Categories.Add(category);

                try
                {
                    _context.SaveChanges();
                    Console.WriteLine("Category saved successfully.");
                    TempData["SuccessMessage"] = "Category created successfully!";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving category: {ex.Message}");
                    TempData["ErrorMessage"] = "An error occurred while creating the category.";
                    return View("Categories/Create", category);
                }

                return RedirectToAction("Categories");
            }

            Console.WriteLine("ModelState is invalid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }

            return View("Categories/Create", category); 
        }
        // Editing Category
        [HttpGet("Categories/Edit/{id}")]
        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View("Categories/Edit", category); 
        }

        [HttpPost("Categories/Edit/{id}")]
        public IActionResult EditCategory(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
                if (existingCategory == null) return NotFound();

                existingCategory.Name = category.Name;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction("Categories");
            }
            return View("Categories/Edit", category); 
        }
        // Deleting Category
        [HttpGet("Categories/Delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View("Categories/Delete", category); // Looks for /Views/Admin/Categories/Delete.cshtml
        }

        [HttpPost("Categories/Delete/{id}")]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            return RedirectToAction("Categories");
        }

        // Products Management
        [HttpGet("Products")]
        public IActionResult Products()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View("Products/Index", products); 
        }

        [HttpGet("Products/Create")]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View("Products/Create"); 
        }

        //Create product
        [HttpPost("Products/Create")]
        public IActionResult CreateProduct(Product product)
        {
            Console.WriteLine("CreateProduct POST method called.");

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Product Name: {product.Name}, Price: {product.Price}, CategoryId: {product.CategoryId}");
                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Products");
            }

            Console.WriteLine("ModelState is invalid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View("Products/Create", product);
        }
        //Edit product
        [HttpGet("Products/Edit/{id}")]
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View("Products/Edit", product); 
        }

        [HttpPost("Products/Edit/{id}")]
        public IActionResult EditProduct(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.CategoryId = product.CategoryId;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Products");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View("Products/Edit", product); 
        }

        //Delete product
        [HttpGet("Products/Delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products
                          .Include(p => p.Category) //  load the Category
                          .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return View("Products/Delete", product);
        }

        [HttpGet("ContactSubmissions")]
        public IActionResult ContactSubmissions()
        {
            // Fetch all contact form
            var submissions = _context.ContactUs.ToList();
            return View("ContactSubmissions", submissions); 
        }

        [HttpGet("Orders")]
        public IActionResult Orders()
        {
            // Fetch all orders 
            var orders = _context.Orders
                .Include(o => o.OrderItems) 
                .ToList();

            return View("Orders", orders); 
        }

        [HttpGet("Orders/Details/{id}")]
        public IActionResult OrderDetails(int id)
        {
            // Fetch the order by ID
            var order = _context.Orders
                .Include(o => o.OrderItems) 
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound(); 
            }

            return View("OrderDetails", order); 
        }




        [HttpPost("Products/Delete/{id}")]
        public IActionResult DeleteProductConfirmed(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                Console.WriteLine($"Product with ID {id} deleted successfully.");
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            return RedirectToAction("Products");
        }

    }
}
