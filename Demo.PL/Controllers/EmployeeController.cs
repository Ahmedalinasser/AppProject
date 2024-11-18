using AutoMapper;
using Demo.BLL.Interface;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController( IMapper mapper , IUnitOfWork unitOfWork)
        {

            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            // ViewBag.Message = "Hello From View Bag";
            IEnumerable<Employee> empModule;
            if (string.IsNullOrEmpty(SearchValue))
            {
             empModule = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                empModule = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            }
           var mappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(empModule);
            return View( mappedEmployee);
        }

        public IActionResult Create()
        {
            //ViewBag.Departments = _dep.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if(ModelState.IsValid)
            {
                employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image , "Images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(employeeVM);
                
                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                var result = await _unitOfWork.SaveChangesInDbAsync();
                if (result > 0)
                    TempData["M3"] = "Employee was Created successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(employeeVM);
            }

        }

        public async Task <IActionResult> Details([FromRoute] int? id , string ActionName = "Details")
        {
            if (id is null)
                return BadRequest();
            var empModule = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (empModule is null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(empModule);
            return View( ActionName, mappedEmployee);   
        }


        public async Task<IActionResult> Update ([FromRoute] int? id)
        {
            ViewBag.Departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update (EmployeeViewModel  employeeVM , [FromRoute] int? id)
        {
            if (id is null)
                return  BadRequest();
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image, "Images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                await _unitOfWork.SaveChangesInDbAsync();
                return RedirectToAction("Index");
            }
            else
                return View(employeeVM);
 
        }


        public async Task<IActionResult> Delete([FromRoute] int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (EmployeeViewModel employeeVM, [FromRoute] int? id)
        {
            if (id != employeeVM.Id)
                return NotFound();

            try 
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                await _unitOfWork.SaveChangesInDbAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);

            }
        }
        
    }
}
