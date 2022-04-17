using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(loginValidator => loginValidator.UserName)
                .NotEmpty();

            RuleFor(loginValidator => loginValidator.Password)
                .MinimumLength(6)
                .NotEmpty();
        }


    }
}