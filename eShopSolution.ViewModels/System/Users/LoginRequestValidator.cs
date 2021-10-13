using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Yêu cầu nhập tài khoản");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Yêu cầu nhập mật khẩu")
                .MinimumLength(8).WithMessage("Mật khẩu ít nhất 8 ký tự");
        }

    }
}
