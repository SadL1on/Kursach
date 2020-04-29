using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class SupplierViewModel
    {

        public int IdSuplier { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]

        [Display(Name = "Компания")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        
        [Display(Name = "Мобильный телефон")]
     //   [MinLength(11,ErrorMessage ="Миниум 11 символов")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Номер карты")]
        [MinLength (16,ErrorMessage ="Какрта должна содеражить больше 16 символов")]
        public string NumberCard { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Документы")]
        public IFormFile Docyment { get; set; }
        public bool Cheak { get; set; } = false;
    }
}
