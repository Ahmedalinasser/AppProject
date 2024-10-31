using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name IS Required")]
        [MinLength(2, ErrorMessage = "Min length is 2 chars ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Code is required")]
        [MinLength(1, ErrorMessage = "Min length is 1 chars ")]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }



    }
}
