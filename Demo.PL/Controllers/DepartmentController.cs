using AutoMapper;
using Demo.BLL.Interface;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using System.Collections.Generic;
using System.Web;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IMapper mapper, IUnitOfWork unitOfWork)
        {            
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //ViewData["Message"] = "Hello From View Data";
            var department = _unitOfWork.DepartmentRepository.GetAll();
            var mappedModel = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            //var mappedModel = _mapper.Map<IEnumerable<Department> ,IEnumerable<DepartmentViewModel>> (department);
            return View(mappedModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            //IEnumerable<Department> department; 
            if (ModelState.IsValid)
            {
                var mappedModel = _mapper.Map<DepartmentViewModel , Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Add(mappedModel);
                var result = _unitOfWork.SaveChangesInDb(); 
                if (result >0)
                    TempData["M2"] = "Department was Created successfully";
                    
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(departmentVM);
            }
        }

        public IActionResult Details(int? id , string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            var mappedModel = _mapper.Map<Department , DepartmentViewModel>(department);
            if (mappedModel is null)
                return NotFound();

            return View( viewName , mappedModel);
        }

        public IActionResult Update(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var dep = _departmentRepository.GetById(id.Value);
            //if (dep is null)
            //    return NotFound();

            return Details(id , "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DepartmentViewModel departmentVM , [FromRoute] int id)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid) 
            {
                var mappedModel = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Update(mappedModel);
                _unitOfWork.SaveChangesInDb();
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                return View(departmentVM);
            }

        }
        public IActionResult Delete (int? id)
        { 
            return Details(id, "Delete");
        } 

        [HttpPost]
        public IActionResult Delete (DepartmentViewModel depvm , [FromRoute] int id)
        {
            if (id != depvm.Id)
                return NotFound(); 

            try
            {
                var mappedModel = _mapper.Map<DepartmentViewModel , Department>(depvm);
                _unitOfWork.DepartmentRepository.Delete(mappedModel);
                _unitOfWork.SaveChangesInDb();
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
