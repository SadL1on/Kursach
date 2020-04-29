using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.ViewModels
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public IFormFile Image { get; set; }
        public int IdProd { get; set; }
        public string IdSupplier { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public int Cost { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public int Col { get; set; }
        
        
    }
}
