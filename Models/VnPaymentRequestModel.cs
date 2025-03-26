namespace OnlineMovieTicket.Models
{
    public class VnPaymentRequestModel
    {
        public string OrderId { get; set; }       // Mã đơn đặt vé
        public string FullName { get; set; }      // Tên người dùng
        public string Description { get; set; }   // Mô tả vé đặt
        public decimal Amount { get; set; }       // Tổng số tiền thanh toán
        public DateTime CreatedDate { get; set; } // Ngày tạo đơn
    }

    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }          // Trạng thái thanh toán (thành công/thất bại)
        public string PaymentMethod { get; set; }  // Phương thức thanh toán (VNPay)
        public string OrderDescription { get; set; } // Mô tả về vé
        public string OrderId { get; set; }        // Mã đơn đặt vé
        public string TransactionId { get; set; }  // Mã giao dịch VNPay
        public string Token { get; set; }          // Mã token bảo mật
        public string VnPayResponseCode { get; set; } // Mã phản hồi từ VNPay
        public string VnPaySecureHash { get; set; }
    }
}
