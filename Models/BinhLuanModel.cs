using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
namespace OnlineMovieTicket.Models
{
    public class BinhLuanModel
    {
        [Key]
        public int MaBinhLuan { get; set; }

        [Required]
        [StringLength(500)]
        public string NoiDungBinhLuan { get; set; }

        [Required]
        public DateTime NgayBinhLuan { get; set; }

        [Required]
        public string TenNguoiBinhLuan { get; set; }


        [Required]
        public string MaNguoiBinhLuan { get; set; }
        [ForeignKey("MaNguoiBinhLuan")]
        public AppNguoiDung NguoiBinhLuan { get; set; }


        [Required]
        public int MaPhim { get; set; }
        [ForeignKey("MaPhim")]
        public PhimModel Phim { get; set; }

    }
}
