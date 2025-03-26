using OnlineMovieTicket.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace OnlineMovieTicket.Models
{
    public class SuatChieuModel
    {
        [Key]
        public int MaSuatChieu { get; set; }


        [Display(Name = "Phim")]
        public int MaPhim { get; set; }
        [ForeignKey("MaPhim")]
        public PhimModel Phim { get; set; }


        [Display(Name = "Phòng chiếu")]
        public int MaPhong { get; set; }
        [ForeignKey("MaPhong")]
        public PhongModel Phong { get; set; }


        [Display(Name = "Ngày chiếu")]
        public int MaNgayChieu { get; set; }
        [ForeignKey("MaNgayChieu")]
        public NgayChieuModel NgayChieu { get; set; }


        [Display(Name = "Thời gian chiếu")]
        public int MaThoiGianChieu { get; set; }
        [ForeignKey("MaThoiGianChieu")]
        public ThoiGianChieuModel ThoiGianChieu { get; set; }


        [Display(Name = "Giá vé của suất chiếu")]
        public decimal GiaVeSuatChieu { get; set; }
    }
}


