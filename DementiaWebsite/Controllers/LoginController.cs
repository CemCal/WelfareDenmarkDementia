using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DementiaWebsite.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public LoginController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet, Route("create")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateUser(Person person)
        {
            if (!ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                {
                    UserName = person.Email,
                    Email = person.Email,
                };
                var result = await _userManager.CreateAsync(user, person.PassWord);

                if (result.Succeeded)
                {
                    return RedirectToAction("ConfirmSignUp", "Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }
        [Route("confirm")]
        public IActionResult ConfirmSignUp()
        {
            return View();
        }

    }
}
