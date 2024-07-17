using System.ComponentModel.DataAnnotations;

namespace HDigital.Models
{
    public class Resurse
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public  string Tip { get; set; }
        public  string Nume { get; set; }  
        public int Cantitate { get; set; }
        public  string UnitateDeMasura { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataAchizitie { get; set; }

        public double PretAchizitie { get; set; }
    }
}
