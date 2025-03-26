using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMovieTicket.Models
{
    public class SuKienModel
    {
        [Key]
        public int MaSuKien { get; set; }

        [Required]
        public string TenSuKien { get; set; }

        [Required]
        public string AnhSuKien { get; set; }

        [Required]
        public string NoiDungSuKien { get; set; }

        [Required]
        public DateTime NgayDangSuKien { get; set; }


        [Display(Name = "Phim")]
        public int MaPhim { get; set; }
        [ForeignKey("MaPhim")]
        public PhimModel Phim { get; set; }

    }
}

