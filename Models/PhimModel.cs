using OnlineMovieTicket.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace OnlineMovieTicket.Models
{
    public class PhimModel
    {
        [Key]
        public int MaPhim { get; set; }

        public string? AnhPhim { get; set; }
        public string? TrailerPhim { get; set; }

        [Required]
        public bool TinhTrang { get; set; }
        [Required]
        public string TenPhim { get; set; }
        [Required]
        public string NoiDungPhim { get; set; }
        [Required]
        public string TheLoai { get; set; }
        [Required]
        public string QuocGia { get; set; }
        [Required]
        public string DaoDien { get; set; }
        [Required]
        public string DienVien { get; set; }
        [Required]
        public string ThoiLuong { get; set; }
        [Required]
        public DateTime NgayKhoiChieu { get; set; }
    }
}