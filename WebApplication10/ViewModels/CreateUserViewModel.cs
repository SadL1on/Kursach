using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Реквизит обязателен для заполнения")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения")]

        public string Password { get; set; }
       
    }
}
