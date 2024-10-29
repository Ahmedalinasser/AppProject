using Demo.BLL.Interface;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _emp;

        public EmployeeController(IEmployeeRepository emp)
        {
            this._emp = emp;
        }
        public IActionResult Index()
        {
           var empModule = _emp.GetAll();
            return View( empModule);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _emp.Add(employee);
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


        public IActionResult Edit ([FromRoute] int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (Employee employee , [FromRoute] int? id)
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


        public IActionResult Remove([FromRoute] int? id)
        {
            return Details(id, "Remove");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove (Employee employee, [FromRoute] int? id)
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
