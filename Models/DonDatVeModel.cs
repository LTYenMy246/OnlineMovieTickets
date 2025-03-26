using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Models
{
    public class DonDatVeModel
    {
        [Key]
        public int MaDonDatVe { get; set; }


        [Required]
        public string MaNguoiDung { get; set; }
        [ForeignKey("MaNguoiDung")]
        public AppNguoiDung NguoiDung { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày đặt")]
        public DateTime NgayDat { get; set; }

    }
}
