﻿using System;

namespace Demo.PL.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();    
        }

        public string Id {get;set;}
        public string RoleName { get;set;}
    }
}
