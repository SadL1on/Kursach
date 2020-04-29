using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class ProductsViewModelcs
    {
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public string NameProduct { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public int Col { get; set; }
        public string IdSuplier { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public IFormFile Path { get; set; }
    }
}
