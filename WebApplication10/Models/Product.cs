
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Zero.Models
{
    public class Product
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProd { get; set; }
        public string IdSupplier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public int Col { get; set; }
        public int startCol { get; set; }
        public byte[] Image { get; set; }

        public int Colprod()
        {
            return Math.Abs( startCol - Col);
        }
        public int Itogo()
        {
            return (Math.Abs(startCol - Col)) * Cost;
        }
    }
}
