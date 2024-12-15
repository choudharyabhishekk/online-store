using GadgetStore.Data;
using GadgetStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace GadgetStore.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
       
        private readonly GadgetStoreDbContext _context;

        public ContactController(GadgetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                _context.ContactUs.Add(model);
                await _context.SaveChangesAsync();
                ViewData["SuccessMessage"] = "Thank you for reaching out. We will get back to you soon.";
                return View();
            }

            return View(model);
        }
    }
}