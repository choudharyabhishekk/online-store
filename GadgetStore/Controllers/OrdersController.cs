using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GadgetStore.Data;
using GadgetStore.Models;
using System.Linq;

namespace GadgetStore.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrdersController : Controller
    {
        private readonly GadgetStoreDbContext _context;

        public OrdersController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;
            var orders = _context.Orders
                .Where(o => o.Email == userEmail)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.TotalAmount,
                    o.OrderItems
                })
                .ToList();

            if (!orders.Any())
            {
                ViewBag.NoOrders = true;
                return View();
            }

            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == oi.ProductId)?.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();

            ViewBag.NoOrders = false;
            return View(orderViewModels);
        }
    }
}
