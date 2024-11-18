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

        public async Task AddAsync(T item)
        {
           await _context.AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Remove(item); 
        }

        public async Task <IEnumerable<T>> GetAllAsync()
        {
            if ( typeof(T) == typeof(Employee) )
            {
                return  (IEnumerable<T>) await _context.Employees.Include(e => e.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }



        public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);
           
        

        public void Update(T item)
        {
            _context.Update(item);
        }
    }
}
