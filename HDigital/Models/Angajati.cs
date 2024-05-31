using System.ComponentModel.DataAnnotations;

namespace HDigital.Models
{
    public class Angajati
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Nume { get; set; }
        public string Pozitie { get; set; }
        public double Salariu { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }
        
        public DateTime ExpirareContract { get; set;}
    }
}
