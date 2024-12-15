using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace GadgetStore.Controllers
{
    [Authorize]
    [Route("checkout")]
    public class CheckoutController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public CheckoutController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var total = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.Total = total;
            return View(cart);
        }

        [HttpPost("process")]
        public IActionResult ProcessOrder(string customerName, string email, string address, string phone, string paymentMethod)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                CustomerName = customerName,
                Email = email,
                Address = address,
                Phone = phone,
                PaymentMethod = paymentMethod,
                TotalAmount = cart.Sum(item => item.Product.Price * item.Quantity),
                OrderItems = cart.Select(item => new OrderItem
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Clear the cart
            HttpContext.Session.Remove("Cart");

            TempData["SuccessMessage"] = $"Thank you, {customerName}! Your order has been placed.";
            return RedirectToAction("Index", "Products");
        }

    }
}
