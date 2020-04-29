using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zero.Models
{
    public class Buyer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdBuyer { get; set; }
        public string Name { get; set; }
        public string Adrees { get; set; }

        public int Card { get; set; }
    }
}