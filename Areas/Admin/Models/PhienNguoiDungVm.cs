using System.ComponentModel.DataAnnotations;

namespace OnlineMovieTicket.Areas.Admin.Models
{
	public class PhienNguoiDungVm
	{
		[Required]
		[Display(Name = "Người dùng")]
		public string UserId { get; set; }
		[Required]
		[Display(Name = "Vai trò")]
		public string RoleId { get; set; }
	}
}

