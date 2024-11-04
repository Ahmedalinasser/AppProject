using Demo.BLL.Interface;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using System.Web;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentRepository;

        //public object ModelSatate { get; private set; }

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            //ViewData["Message"] = "Hello From View Data";
            var department = _departmentRepository.GetAll();
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                
                var result = _departmentRepository.Add(department);
                if (result >0)
                    TempData["M2"] = "Department was Created successfully";
                    
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(department);
            }
        }

        public IActionResult Details(int? id , string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();

            var department = _departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();

            return View( viewName ,department);
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
        public IActionResult Update(Department department , [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
                _departmentRepository.Update(department);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete (int? id)
        { 
            return Details(id, "Delete");
        } 

        [HttpPost]
        public IActionResult Delete (Department dep , [FromRoute] int id)
        {
            if (id != dep.Id)
                return NotFound(); 

            try
            {
                _departmentRepository.Delete(dep);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dep);
                
            }

        }


    }
}
