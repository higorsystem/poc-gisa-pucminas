using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidator()
        {
            RuleFor(resetPasswordValidator => resetPasswordValidator.UserName)
                .NotEmpty();

            RuleFor(resetPasswordValidator => resetPasswordValidator.Password)
                .NotEmpty();

            RuleFor(resetPasswordValidator => resetPasswordValidator.Code)
                .NotEmpty();
        }
    }
}