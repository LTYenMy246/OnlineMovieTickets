using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class GheSuatChieuModel
    {
        [Key]
        public int MaGheSuatChieu { get; set; }

        [Display(Name = "Suất Chiếu")]
        public int MaSuatChieu { get; set; }
        [ForeignKey("MaSuatChieu")]
        public SuatChieuModel SuatChieu { get; set; }

        [Display(Name = "Ghế")]
        public int MaGhe { get; set; }
        [ForeignKey("MaGhe")]
        public GheModel Ghe { get; set; }

        [Required]
        [Display(Name = "Trạng thái ghế")]
        public bool TrangThaiGhe { get; set; }

    }
}
