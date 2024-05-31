using System.ComponentModel.DataAnnotations;

namespace HDigital.Models
{
    public class Vehicule
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Categorie { get; set; }

        public string Brand { get; set; }   
        public string Model { get; set; }

        public int Putere { get; set; }
        public int OreFunctionare { get; set; }
        public int AnFabricatie { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataAchizitie { get; set; }
        public double PretAchizitie { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UltimaMentenanta {  get; set; }

    }
}
