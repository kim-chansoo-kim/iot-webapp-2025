using System.ComponentModel.DataAnnotations;

namespace WebApiApp03.Models
{
    public class iot_datas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Sensing_Dt { get; set; }
        [Required]
        public string Loc_Id { get; set; }

        public float Temp { get; set; }

        public float Humid { get; set; }
    }
}
