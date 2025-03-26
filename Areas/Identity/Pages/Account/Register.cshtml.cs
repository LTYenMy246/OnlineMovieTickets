// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppNguoiDung> _signInManager;
        private readonly UserManager<AppNguoiDung> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserStore<AppNguoiDung> _userMovie;
        private readonly IUserEmailStore<AppNguoiDung> _emailMovie;

        public RegisterModel(
            UserManager<AppNguoiDung> userManager,
            IUserStore<AppNguoiDung> userMovie,
            SignInManager<AppNguoiDung> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userMovie = userMovie;
            _emailMovie = GetEmailStore();
        }

        private IUserEmailStore<AppNguoiDung>? GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppNguoiDung>)_userMovie;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Họ")]
            public required string Ho { get; set; }

            [Required]
            [Display(Name = "Tên")]
            public required string Ten { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public required string UserName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public required string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Nhập chưa đúng cú pháp", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public required string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            public required string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        //public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/");
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //    //ModelState.Clear();
        //    if (ModelState.IsValid)
        //    {
        //        var user = CreateUser();

        //        user.Ho = Input.Ho;
        //        user.Ten = Input.Ten;
        //        await _userMovie.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
        //        await _emailMovie.SetEmailAsync(user, Input.Email, CancellationToken.None);
        //        var result = await _userManager.CreateAsync(user, Input.Password);
        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(user, "Người dùng");
        //            _logger.LogInformation("Khách hàng tạo tài khoản người dùng mới với mật khẩu.");
        //            var userId = await _userManager.GetUserIdAsync(user);
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
        //                protocol: Request.Scheme);

        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
        //            }
        //            else
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return LocalRedirect(returnUrl);
        //            }
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            if (error.Code == "DuplicateUserName")
        //            {
        //                ModelState.AddModelError("Input.UserName", "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
        //            }
        //            else if (error.Code == "DuplicateEmail")
        //            {
        //                ModelState.AddModelError("Input.Email", "Email đã tồn tại, vui lòng chọn email khác.");
        //            }
        //            else if(error.Code == "PasswordTooShort" ||
        //            error.Code == "PasswordRequiresNonAlphanumeric" ||
        //            error.Code == "PasswordRequiresDigit" ||
        //            error.Code == "PasswordRequiresUpper")
        //            {
        //                ModelState.AddModelError("Input.Password", "*Có ít nhất 6 ký tự, có chữ số và chữ viết hoa.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //    }
        //    return Page();

        //}
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Ho = Input.Ho;
                user.Ten = Input.Ten;
                await _userMovie.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailMovie.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Người dùng");
                    _logger.LogInformation("Khách hàng tạo tài khoản người dùng mới với mật khẩu.");
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn",
                        $"Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // Lưu thông báo vào TempData
                        TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
                    }
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError("Input.UserName", "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
                    }
                    else if (error.Code == "DuplicateEmail")
                    {
                        ModelState.AddModelError("Input.Email", "Email đã tồn tại, vui lòng chọn email khác.");
                    }
                    else if (error.Code == "PasswordTooShort" ||
                             error.Code == "PasswordRequiresNonAlphanumeric" ||
                             error.Code == "PasswordRequiresDigit" ||
                             error.Code == "PasswordRequiresUpper")
                    {
                        ModelState.AddModelError("Input.Password", "*Có ít nhất 6 ký tự, có chữ số và chữ viết hoa.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Page();
        }
        private AppNguoiDung CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppNguoiDung>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppNguoiDung)}'. " +
                    $"Ensure that '{nameof(AppNguoiDung)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}