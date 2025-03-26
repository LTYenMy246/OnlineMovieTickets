using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class ChiTietDatVeModel
    {
        [Key]
        public int MaChiTietDatVe { get; set; }


        [Display(Name = "Mã đơn đặt vé")]
        public int MaDonDatVe { get; set; }
        [ForeignKey("MaDonDatVe")]
        public DonDatVeModel DonDatVe { get; set; }


        [Display(Name = "Mã ghế suất chiếu")]
        public int MaGheSuatChieu { get; set; }
        [ForeignKey("MaGheSuatChieu")]
        public GheSuatChieuModel GheSuatChieu { get; set; }


        [Display(Name = "Giá vé")]
        public decimal GiaVe { get; set; }
    }
}
