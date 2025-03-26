using OnlineMovieTicket.Models;
using OnlineMovieTicket.Utilities;

namespace OnlineMovieTicket.Services
{
    public class VnPayService : IVnPayService
    {
        // Cấu hình và các phương thức cần thiết
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var txnRef = DateTime.Now.Ticks.ToString(); // Mã tham chiếu giao dịch

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString()); // Số tiền tính bằng VNĐ và nhân với 100
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn vé: {model.Description}");
            vnpay.AddRequestData("vnp_OrderType", "movie");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", txnRef);

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
            Console.WriteLine(paymentUrl);
            return paymentUrl;
        }

        // Xử lý phản hồi từ VNPay sau khi thanh toán
        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            //var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);

            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                };
            }

            // Trả về kết quả thành công
            return new VnPaymentResponseModel
            {
                Success = true, // Kiểm tra mã phản hồi từ VNPay
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
        public static class Utils
        {
            public static string GetIpAddress(HttpContext context)
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
                return ipAddress ?? "0.0.0.0"; // Hoặc bất kỳ giá trị mặc định nào bạn muốn
            }
        }
    }
}
