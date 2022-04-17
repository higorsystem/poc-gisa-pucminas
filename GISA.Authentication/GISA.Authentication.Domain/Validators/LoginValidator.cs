using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        public RefreshTokenValidator()
        {
            RuleFor(loginValidator => loginValidator.UserName)
                .NotEmpty();

            RuleFor(loginValidator => loginValidator.Token)                
                .NotEmpty();
        }


    }
}