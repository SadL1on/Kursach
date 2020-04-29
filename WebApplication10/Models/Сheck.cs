using System;
using System.ComponentModel.DataAnnotations;

namespace Zero.Models
{
    public class Сheck
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Sum { get; set; }
        public string IdBuyer { get; set; }
        public string IdSup { get; set; }
        public string NameSup { get; set; }
        public string NameBuyer { get; set; }
        public bool status { get; set; }
        public bool operation { get; set; } = false;
        public bool work { get; set; } = false;
    }
}