namespace OnlineMovieTicket.ViewModels
{
    public class DonDatVeViewModel
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public DateTime NgayChieu { get; set; }
        public string TenRap { get; set; }
        public string TenPhong { get; set; }
        public TimeSpan ThoiGianChieu { get; set; }
        public List<string> GheDaChon { get; set; }
        public decimal TongTien { get; set; }
    }
}
