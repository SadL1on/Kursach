using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Zero.Models;

namespace WebApplication10.Models
{
    public class CartItems
    {
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
