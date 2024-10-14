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
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly  App3TierArch _dbContext;

         public DepartmentRepository(App3TierArch dbContext) 
        {
           _dbContext = dbContext;
        }

        public int Add(Department department)
        {
            _dbContext.Add(department);
            return _dbContext.SaveChanges();
        }

        public int Delete(Department department)
        {
            _dbContext.Remove(department); 
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
        =>_dbContext.Departments.ToList();
        

        public Department GetById(int id) 
            =>_dbContext.Departments.Find(id);

        public int Update(Department department)
        {
            _dbContext.Update(department);
            return _dbContext.SaveChanges();
        }
        
    }
}
