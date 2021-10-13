using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            //Cac validator co san
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password is at least 8 characters");

            RuleFor(x => x.FirtName).NotEmpty().WithMessage("FirtName is required")
                .MaximumLength(200).WithMessage("FirtName can not over 200 characters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required")
                .MaximumLength(200).WithMessage("LastName can not over 200 characters");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Birthday cannot greater than 120 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format is not matched");
            //Tao validator Custom duoc tuy bien  theo nhu cau
            RuleFor(x => x).Custom((request, context) =>
            { //request = RegisterRequest do truyen vao class AbstractValidator<RegisterRequest>; context của Validator
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("The confirm password is not matched with the password");
                }
            });
        }
    }
}