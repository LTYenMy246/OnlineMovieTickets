using OnlineMovieTicket.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnlineMovieTicket.Models
{
    public class RapModel
    {
        [Key]
        public int MaRap { get; set; }
        [Required]
        public string TenRap { get; set; }
        [Required]
        public string DiaChiRap { get; set; }
    }
}