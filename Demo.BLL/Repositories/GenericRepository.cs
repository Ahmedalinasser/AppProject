using Demo.BLL.Interface;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        private readonly App3TierArch _context;

        public GenericRepository(App3TierArch context )
        {
            this._context = context;
        }

        public int Add(T item)
        {
            _context.Add(item);
            return _context.SaveChanges();
        }

        public int Delete(T item)
        {
            _context.Remove(item); 
            return _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            if ( typeof(T) == typeof(Employee) )
            {
                return (IEnumerable<T>)_context.Employees.Include(e => e.Department).ToList();
            }
            return _context.Set<T>().ToList();
        }



        public T GetById(int id)
        => _context.Set<T>().Find(id);
           
        

        public int Update(T item)
        {
            _context.Update(item);
            return _context.SaveChanges();
        }
    }
}
