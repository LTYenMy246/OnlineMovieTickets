using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class ThoiGianChieuModel
    {
        [Key]
        public int MaThoiGianChieu { get; set; }
        [Required]
        public TimeSpan ThoiGianChieu { get; set; }
    }
}
