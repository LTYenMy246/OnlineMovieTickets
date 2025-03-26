using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class PhongModel
    {
        [Key]
        public int MaPhong { get; set; }

        [Required]
        public string TenPhong { get; set; }


        [Display(Name = "Rạp")]
        public int MaRap { get; set; }
        [ForeignKey("MaRap")]
        public RapModel Rap { get; set; }


        [Display(Name = "Số lượng ghế")]
        public int SoLuongGhe { get; set; }

    }
}
