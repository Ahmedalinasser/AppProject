using AutoMapper;
using Demo.BLL.Interface;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController( UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        #region Register
        // Register
        public IActionResult Register ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel modelVM)
        {
            //P@ssw0rd

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FName = modelVM.FName,
                    LName = modelVM.LName,
                    IsAgree = modelVM.IsAgree,
                    Email = modelVM.Email,
                    UserName = modelVM.Email.Split('@')[0]

                };
                var result = await _userManager.CreateAsync(user, modelVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(modelVM); 

        }

        #endregion

        #region Login
        // Login
        public IActionResult Login ()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login (LoginViewModel modelVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(modelVM.Email);
                if (user is not null)
                {
                    var passwordResult = await _userManager.CheckPasswordAsync(user, modelVM.Password);
                    //check on pass
                    if (passwordResult)
                    {
                       var LoginResult =  await _signInManager.PasswordSignInAsync(user, modelVM.Password , modelVM.RememberMe , false);
                        if (LoginResult.Succeeded)
                        {
                            return RedirectToAction("Index" , "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty , "Incorrect Password ");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is Not existed  ");
                }
            }
           
            return View(modelVM);
        }


        #endregion

        #region Sign Out
        // Sign Out

        #endregion

        #region Forget Password
        // Forget Password

        #endregion

        #region Reset Password
        // Reset Password

        #endregion







    }
}
