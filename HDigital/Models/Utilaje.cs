using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace HDigital.Models
{
    public class Utilaje
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Categorie { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        public int PutereNecesara { get; set; }
        public int Greutate { get; set; }
        public int AnFabricatie { get; set; }
        public int OreFunctionare { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataAchizitie { get; set; }

        public double PretAchizitie { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UltimaMentenanta { get; set; }
    }
}
