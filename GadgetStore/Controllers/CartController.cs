using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GadgetStore.Controllers
{

    [Route("cart")]
    public class CartController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public CartController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // Add to Cart
        [HttpPost("add")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (quantity < 1 || quantity > 10)
            {
                TempData["ErrorMessage"] = "Invalid quantity. Please select a quantity between 1 and 10.";
                return RedirectToAction("Details", "Products", new { id = productId });
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index", "Products");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.Product.Id == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = cartItem.Quantity + quantity > 10 ? 10 : cartItem.Quantity + quantity;
            }
            else
            {
                cart.Add(new CartItem { Product = product, Quantity = quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            TempData["SuccessMessage"] = $"{quantity} unit(s) of {product.Name} added to your cart.";
            return RedirectToAction("Details", "Products", new { id = productId });
        }

        // Remove from Cart
        [HttpPost("remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.Product.Id == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                TempData["SuccessMessage"] = $"{cartItem.Product.Name} removed from your cart.";
            }
            else
            {
                TempData["ErrorMessage"] = "Product not found in cart.";
            }

            return RedirectToAction("Index");
        }
    }
}
