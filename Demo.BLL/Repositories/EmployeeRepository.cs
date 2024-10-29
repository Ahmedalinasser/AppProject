using Demo.BLL.Interface;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository :  GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly App3TierArch _context;

        public EmployeeRepository(App3TierArch context):base(context) 
        {
            this._context = context;
        }

        public IQueryable<Employee> GetEmployeesByDepartment(string address)
        
            =>_context.Employees.Where(Emp => Emp.Address == address);
        

    }
}
