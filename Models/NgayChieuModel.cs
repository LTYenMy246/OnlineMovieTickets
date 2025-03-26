using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMovieTicket.Models
{
    public class NgayChieuModel
    {
        [Key]
        public int MaNgayChieu { get; set; }

        [Required]
        public DateOnly NgayChieu { get; set; }

        internal string ToString(string v)
        {
            throw new NotImplementedException();
        }
    }
}
