﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models; 

namespace Demo.BLL.Interface
{
    public interface IDepartmentRepository
    {

        IEnumerable<Department> GetAll();
        Department GetById(int id);

        int Add(Department department);
        int Update(Department department);
        int Delete(Department department);


    }
}