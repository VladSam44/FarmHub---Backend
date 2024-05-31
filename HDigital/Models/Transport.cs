using OpenAI_API.Moderation;
using System.ComponentModel.DataAnnotations;

namespace HDigital.Models
{
    public class Transport
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Categorie { get; set; }
        public string Brand { get; set; }

        public int Capacitate { get; set; }

       
        public int AnFabricatie { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataAchizitie {  get; set; }
        public double PretAchizitie { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UltimaMentenanta { get; set; }
        public string TipAtasament { get; set; }
    }
}
