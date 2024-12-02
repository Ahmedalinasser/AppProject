using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController( UserManager<ApplicationUser> userManager , IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }



        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                //var Users = await _userManager.Users.ToListAsync();
                //foreach (var usermodel in Users)
                //{
                //    var mappedUser = new UsersViewModel()
                //    {
                //        Fname = usermodel.FName,
                //        Lname = usermodel.LName,
                //        Email = usermodel.Email,
                //        PhoneNumber = usermodel.PhoneNumber,
                //        Role = _userManager.GetRolesAsync(usermodel).Result
                //    };
                //}


                /* var Users = _userManager.Users.ToListAsync();*/
                // it will return a task of List of ApplicationUser "List<ApplicationUser>"
                var users = await _userManager.Users.Select(u => new UsersViewModel()// while this will return 
                {     Id =  u.Id,                                  //task of List of UsersViewModel "List<UsersViewModel>" which is needed for 
                    Fname = u.FName,                    // the View to return a ViewModel Object or list 
                    Lname = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = _userManager.GetRolesAsync(u).Result //<- 'Result' is used to make the method sync not Async 
                }).ToListAsync();
                return View(users);
                /* A very Good note you always need to check what is the return type and what it will be used for 
                in this case we used the manual mapping because it will return "List<ApplicationUser>" and 
                we need to map it to be ViewMode Object*/

            }
            else
            {
                var user = await _userManager.FindByEmailAsync(searchValue);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email was not found ");
                    var users = await _userManager.Users.Select(u => new UsersViewModel()
                    {
                        Id = u.Id,
                        Fname = u.FName,             
                        Lname = u.LName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Role = _userManager.GetRolesAsync(u).Result 
                    }).ToListAsync();
                    return View(users);
                }
                
                var MappedUserToUserVM = new UsersViewModel()
                {
                    Id = user.Id,
                    Fname = user.FName,
                    Lname = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = _userManager.GetRolesAsync(user).Result

                };
                return View( new List<UsersViewModel> { MappedUserToUserVM });
            }

        }
    
        public async Task<IActionResult> Details (string id, string viewName ="Details")
        {
            if (id is null)
                return BadRequest();
            var user =await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var MapUserToUserVM = _mapper.Map<ApplicationUser, UsersViewModel>(user);
            return View( viewName , MapUserToUserVM);

                
            
        }


        public async Task<IActionResult> Update([FromRoute]string id)
        {
            return await Details( id ,"Update" );
        }

        [HttpPost]
        public async Task<IActionResult> Update(UsersViewModel userVM, [FromRoute] string id)
        {

            if (id is null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    /// var mappedToUsers = _mapper.Map<UsersViewModel , ApplicationUser>(UserVM); ///
                    /// because ApplicationUser does not represent anything in => data base <=
                    /// i think it needs configuration in profile !!?
                    // mapping will not happen  i think because of the differences in prop in applicationUser 
                    // and it does not has the props of UserManager even though we are using UserManager<ApplicationUser>
                    // which has UserManager props !!!

                    var user = await _userManager.FindByIdAsync(id) ;
                    user.PhoneNumber = userVM.PhoneNumber;
                    user.FName = userVM.Fname;
                    user.LName = userVM.Lname;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));

                }catch(Exception ex)    
                {
                    ModelState.AddModelError(string.Empty , ex.Message);
                }   
            }
            return View(userVM);
        }


        public async Task <IActionResult> Delete([FromRoute] string id)
        { 
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmDelete( string id)
        {
            if (id is null )
                return BadRequest();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));

            }
            catch( Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }



        }

    }
}
