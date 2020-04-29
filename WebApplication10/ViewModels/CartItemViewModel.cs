using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication10.Models;

namespace WebApplication10.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AllId { get; set; }
        public string CartId { get; set; }
        public string ProdDict { get; set; }
        public int CartPrice { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int AllCartPrice { get; set; }
    }
}
