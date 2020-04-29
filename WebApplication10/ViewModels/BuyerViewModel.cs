using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class BuyerViewModel
    {
        public string IdBuyer { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Адрес")]
        public string Adrees { get; set; }
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        [Display(Name = "Номер карты")]
        public int Card { get; set; }
    }
}
