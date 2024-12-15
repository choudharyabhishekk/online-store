using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Users")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View("/Views/Admin/Users/Index.cshtml", users);
        }

       
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            return View("/Views/Admin/Users/Details.cshtml", user);
        }



        // Delete
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            return View("/Views/Admin/Users/Delete.cshtml", user);
        }

        
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            // Check if the user is an admin
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["ErrorMessage"] = "You cannot delete an admin user.";
                return RedirectToAction("Index");
            }

            // Proceed to delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user.";
            }

            return RedirectToAction("Index");
        }


    }
}
