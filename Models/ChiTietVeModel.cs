using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class ChiTietVeModel
    {
        [Key]
        public int MaChiTietVe { get; set; }


        [Display(Name = "Mã đơn đặt vé")]
        public int MaDonDatVe { get; set; }
        [ForeignKey("MaDonDatVe")]
        public DonDatVeModel DonDatVe { get; set; }


        [Display(Name = "Mã ghế")]
        public int MaGhe { get; set; }
        [ForeignKey("MaGhe")]
        public GheModel Ghe { get; set; }


        [Display(Name = "Giá vé")]
        public decimal GiaVe { get; set; }

    }
}
