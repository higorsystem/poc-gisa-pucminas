using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class ForgotPassword : EntityBase
    {
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<ForgotPassword>()
        {
            return new ForgotPasswordValidator().Validate(this);
        }
    }
}