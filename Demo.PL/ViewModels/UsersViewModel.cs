using System.Collections;
using System.Collections.Generic;

namespace Demo.PL.ViewModels
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Role { get; set; }


    }
}
