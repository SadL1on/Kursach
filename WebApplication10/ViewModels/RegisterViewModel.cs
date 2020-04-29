using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Fmail")]
        public string Famil { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [StringLength(100, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
