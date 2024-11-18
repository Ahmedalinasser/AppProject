using AutoMapper;
using Demo.BLL.Interface;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Demo.PL.Controllers
{

   /// [Authorize  ("Admin")]
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IMapper mapper, IUnitOfWork unitOfWork)
        {            
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task <IActionResult> Index()
        {
            //ViewData["Message"] = "Hello From View Data";
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var mappedModel = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            //var mappedModel = _mapper.Map<IEnumerable<Department> ,IEnumerable<DepartmentViewModel>> (department);
            return View(mappedModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            //IEnumerable<Department> department; 
            if (ModelState.IsValid)
            {
                var mappedModel = _mapper.Map<DepartmentViewModel , Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.AddAsync(mappedModel);
                var result = await _unitOfWork.SaveChangesInDbAsync(); 
                if (result >0)
                    TempData["M2"] = "Department was Created successfully";
                    
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(departmentVM);
            }
        }

        public async Task<IActionResult> Details(int? id , string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            var mappedModel = _mapper.Map<Department , DepartmentViewModel>(department);
            if (mappedModel is null)
                return NotFound();

            return View( viewName , mappedModel);
        }

        public async Task<IActionResult> Update(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var dep = _departmentRepository.GetById(id.Value);
            //if (dep is null)
            //    return NotFound();

            return await Details(id , "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DepartmentViewModel departmentVM , [FromRoute] int id)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid) 
            {
                var mappedModel = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Update(mappedModel);
                await _unitOfWork.SaveChangesInDbAsync();
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                return View(departmentVM);
            }

        }
        public async Task<IActionResult> Delete (int? id)
        { 
            return await Details(id, "Delete");
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (DepartmentViewModel depvm , [FromRoute] int id)
        {
            if (id != depvm.Id)
                return NotFound(); 

            try
            {
                var mappedModel = _mapper.Map<DepartmentViewModel , Department>(depvm);
                _unitOfWork.DepartmentRepository.Delete(mappedModel);
                await _unitOfWork.SaveChangesInDbAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(depvm);
                
            }

        }


    }
}
