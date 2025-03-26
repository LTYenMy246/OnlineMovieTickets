namespace OnlineMovieTicket.Models
{
    public class ChiTietDatVeViewModel
    {
        public int MaDonDatVe { get; set; }
        public DateTime NgayDat { get; set; }
        public string TenNguoiDat { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietVeViewModel> ChiTietVe { get; set; }
    }
}
