using Demo.BLL.Interface;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _emp;
        private readonly IDepartmentRepository _dep;

        public EmployeeController(IEmployeeRepository emp , IDepartmentRepository dep)
        {
            this._emp = emp;
            this._dep = dep;
        }
        public IActionResult Index()
        {
           // ViewBag.Message = "Hello From View Bag";

           var empModule = _emp.GetAll();
            return View( empModule);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _dep.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                var result = _emp.Add(employee);
                if (result > 0)
                    TempData["M3"] = "Employee was Created successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(employee);
            }

        }

        public IActionResult Details([FromRoute] int? id , string ActionName = "Details")
        {
            if (id is null)
                return BadRequest();
            var empModule = _emp.GetById(id.Value);
            if (empModule is null)
                return NotFound();

            return View( ActionName, empModule);   
        }


        public IActionResult Update ([FromRoute] int? id)
        {
            ViewBag.Departments = _dep.GetAll();
            return Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update (Employee employee , [FromRoute] int? id)
        {
            if (id is null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _emp.Update(employee);
                return RedirectToAction("Index");
            }
            else
                return View(employee);
 
        }


        public IActionResult Delete([FromRoute] int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete (Employee employee, [FromRoute] int? id)
        {
            if (id != employee.Id)
                return NotFound();

            try 
            {
                _emp.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);

            }
        }
        
    }
}
