﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.ViewModels
{
    public class AddMoneyViewModel
    {
        [Required(ErrorMessage = "Реквизит обязателен для заполнения ")]
        public  int Money { get; set; }
    }
}