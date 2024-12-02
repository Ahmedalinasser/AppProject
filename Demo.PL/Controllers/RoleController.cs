using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            this._roleManager = roleManager;
            this._mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearechValue)
        {
            if (string.IsNullOrEmpty(SearechValue) )
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var mappedToRoleVM =  _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
                return View(mappedToRoleVM );
                
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(SearechValue);
                var mappedToRoleVM = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View( new List<RoleViewModel>() { mappedToRoleVM} );
            }
        }


        public IActionResult Create ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (RoleViewModel roleVM)
        {

            if (ModelState.IsValid)
            {
                var mappedToRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                 await _roleManager.CreateAsync(mappedToRole);
                return RedirectToAction(nameof (Index));
            }
            else
            {
                
            return View(roleVM);
            }

        }


        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            var MapUserToUserVM = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(viewName, MapUserToUserVM);



        }


        public async Task<IActionResult> Update([FromRoute] string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel roleVM, [FromRoute] string id)
        {

            if (id is null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = roleVM.RoleName;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }


        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            if (id is null)
                return BadRequest();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }




    }
}
