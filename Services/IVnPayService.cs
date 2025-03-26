using OnlineMovieTicket.Models;
using OnlineMovieTicket.Utilities;
using System.Text;
namespace OnlineMovieTicket.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
  
}
