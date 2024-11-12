using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {


        public int Id { get; set; }
        [Required(ErrorMessage = "Name IS Required")]
        [MinLength(2, ErrorMessage = "Min length is 2 chars ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [MinLength(1, ErrorMessage = "Min length is 1 chars ")]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }

        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();



    }
}
