using OnlineMovieTicket.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace OnlineMovieTicket.Models
{
    public class GheModel
    {
        [Key]
        public int MaGhe { get; set; }

        [Required]
        public string TenGhe { get; set; }

        [Display(Name = "Phòng chiếu")]
        public int MaPhong { get; set; }
        [ForeignKey("MaPhong")]
        public PhongModel Phong { get; set; }
    }
}