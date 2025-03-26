--- Nội dung của file zip ---
	+ Một file zip chứa source code project (OnlineMovieTicket.zip).
	+ Một file pdf báo cáo đồ án (2151010230_LeThiYenMy.pdf).
	+ Một file README.txt để hướng dẫn và nói về cách chạy chương trình.
	+ Một script SQL (Cinema.sql).


--- Những yêu cầu cần có để chạy website ---
	+ Bạn đã phải cài Visual Studio 2022 (VS2022). Vì phiên bản VS2019 KHÔNG hỗ trợ .NET 8, còn VS2022 thì có hỗ trợ .NET 8
	+ Trong VS2022, đảm bảo là bạn đã cài gói "ASP.NET and web development", nếu chưa cài thì hãy cài về trước khi chạy (Bạn có thể kiểm tra xem VS2022 của mình đã có gói này chưa bằng cách: Visual Studio Installer => Tìm đến VS2022 => Modify)
	+ Bạn đã phải cài SQL Server từ 2019 về sau
	+ Bạn nên cài sẵn SQL Server Management Studio (SSMS) để thuận tiện cho việc sử dụng ứng dụng


--- Các bước thiết lập để chạy chương trình ---
	+ Unzip file source code và để nó vào một thư mục bạn chọn.
	+ Mở project lên bằng Visual Studio 2022
	+ Trong solution explorer, tìm đến file có tên "appsettings.json" và tìm đến phần cấu hình "ConnectionStrings"
		+ Ở phần này có hai giá trị bạn cần quan tâm là "Server=...." và "Database=....". Hiện tại hai giá trị này đang là Server=YENMY và Database=OnlineMovieTicket.
		+ Mặc định, hai giá trị này đang chỉ đến server và cơ sở dữ liệu trên máy của tôi, bạn phải sửa lại hai giá trị đó nếu muốn chạy trên thiết bị của mình.
		+ Mở lên SSMS ở máy bạn và copy phần "Server name", sau đó paste vào phần "Server=...."
		+ Còn giá trị "Database= OnlineMovieTicket": bạn có thể điền bất cứ tên nào cũng được nhưng nên để giống tên cũ để chạy file .SQL không phải chỉnh gì hết. Còn nếu dùng tên 		mới thì chỉnh USE .... trong phải file .sql lại theo tên mới.
		+ Chọn Tool -> NuGet Package Manager -> nhập Update-Database

	+ Sau đó vì dữ liệu khá nhiều nếu muốn trải nghiệm nhanh chóng thì khuyến khích chạy theo cách này dùng file cinema.sql ở SQL Server
		+ Ở SSMS chọn File -> Open -> File...(Ctrl+O) -> mở file Cinema.sql
		+ Chọn tất cả các dòng -> execute: để tạo dữ liệu cho bảng
		+ Sau đó về Visual Studio, tốt nhất bạn nên chọn vào mục Build => Rebuild solution (Hoặc Ctrl + Alt + F7) để build lại solution.
		+ Chạy ứng dụng (F5 / Ctrl + F5)
	+ Bạn có thể tự tạo người dùng để test hoặc dùng những tài khoản trong dữ liệu người dùng (mật khẩu đều là: Abc@123)
	+ Tài khoản admin: 
		+ Tên đăng nhập: Admin
		+ Mật khẩu: Abc@123


--- Lưu ý khi test đặt vé ---
	+ Nếu test đặt vé chọn ngày chiếu không có danh sách thì vào quản lý ngày chiếu tạo 5 ngày tính từ ngày hiện tại trở lên để test( vì thực hiện theo thời gian thực nên dữ liệu là ngày cũ sẽ không hiện lên cho đặt)
	+ Quản lý suất chiếu: để tạo suất chiếu thuộc 1 trong 5 ngày đó( các thuộc tính khác tùy chọn)
	+ Sau đó vào quản lý ghế suất chiếu: tạo mới -> chọn suất chiếu -> tạo (để tạo ghế cho suất chiếu)
	+ Chọn đặt vé lại sẽ thấy dữ liệu ngày


--- Các chức năng có trong ứng dụng ---
	+ Bạn có thể đọc file báo cáo để xem các chức năng, ở đây tôi sẽ tóm tắt một vài chức năng ở đây:
		- Xem/Thêm/Sửa/Xóa Phim, Rạp, Phòng, Ghế, Ngày chiếu, Thời gian chiếu , Suất chiếu,...
		- Thêm/Xóa Vai trò, Xem/Khóa/Mở Khóa/Xóa người dùng.
		- Tìm kiếm phim theo tên phim, xem chi tiết các phim, danh sách phim đang chiếu / sắp chiếu, xem các sự kiện, bình luận phim.
		- Đặt vé (chọn phim, chọn ngày chiếu, chọn suất chiếu theo rạp, chọn ghế), thanh toán bằng trưc tiếp lưu xuống hoặc thanh toán bằng VNPay.
	+ Có nhiều bảng bị số ràng buộc với nhau khi xóa trong ứng dụng, ví dụ như không thể xóa Ghế khi Phòng chưa ghế còn tồn tại.
