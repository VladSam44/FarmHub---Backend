using System.ComponentModel.DataAnnotations;

namespace HDigital.Models
{
    public class Drawing
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; } 
        public User User { get; set; }
        public string Coordinates { get; set; } 

        public string StareTeren { get; set; } 
        public string TipCultura { get; set; } 
        public double Area { get; set; } 
        /*[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]*/
        public DateTime DateAcquired { get; set; }
        /* TEST*/
        /* TEST2*/


        public string UltimaCultura { get; set; }

        public string ProprietarArenda { get; set; }
    }
}
