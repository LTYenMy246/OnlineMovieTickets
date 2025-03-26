namespace OnlineMovieTicket.Models
{
    public class ThanhToanViewModel
    {
        public DateTime NgayDat { get; set; }
        public string NguoiDat { get; set; }
        public decimal TongTien { get; set; }
        public string GheDaChon { get; set; }
        public string TenPhim { get; set; }
        public int MaDonDatVe { get; set; } // Thêm trường mã đơn đặt vé
    }
}
