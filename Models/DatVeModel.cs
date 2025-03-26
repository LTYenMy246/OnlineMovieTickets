using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace OnlineMovieTicket.Models
{
    public class DatVeModel
    {
        [Key]
        public int MaDatVe { get; set; }

        [Display(Name = "Phim")]
        public int MaPhim { get; set; }
        [ForeignKey("MaPhim")]
        public PhimModel Phim { get; set; }

   
        [Display(Name = "Suất chiếu")]
        public int MaSuatChieu { get; set; }
        [ForeignKey("MaSuatChieu")]
        public SuatChieuModel SuatChieu { get; set; }


        [Required]
        public string MaNguoiDung { get; set; }

        [ForeignKey("MaNguoiDung")]
        public AppNguoiDung NguoiDung { get; set; }


        [Display(Name = "Tổng tiền")]
        public int TongTien { get; set; }


        [Display(Name = "Ghế")]
        public int MaGhe { get; set; } // Mã ghế 
        [ForeignKey("MaGhe")]
        public GheModel Ghe { get; set; } // Điều hướng đến Ghế


    }
}

