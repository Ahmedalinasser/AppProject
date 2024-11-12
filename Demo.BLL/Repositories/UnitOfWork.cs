using Demo.BLL.Interface;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
     public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly App3TierArch _dbContext;

        public IDepartmentRepository DepartmentRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }


        public UnitOfWork(App3TierArch dbContext) 
        {
            DepartmentRepository = new DepartmentRepository(dbContext);
            EmployeeRepository = new EmployeeRepository(dbContext)
;
            this._dbContext = dbContext;
        }

        public int SaveChangesInDb()
           => _dbContext.SaveChanges();

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
