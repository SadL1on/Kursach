using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.Models
{
    public class User : IdentityUser
    {
        public int Money { get; set; } = 0;
        public string Famil {get;set;}
        public string Name { get; set; }
        public string Patronymic { get; set; }
    }
}

