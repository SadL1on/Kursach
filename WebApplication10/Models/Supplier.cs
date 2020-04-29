using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zero.Models
{
    public class Supplier
    {
        [Key]
        public string IdSupplier { get; set; }
        public string Company { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string NumberCard { get; set; }
        public byte[] Docyment { get; set; }
        public bool Cheak { get; set; } = false;
        public List<Product> List { get; set; } 
    }
}