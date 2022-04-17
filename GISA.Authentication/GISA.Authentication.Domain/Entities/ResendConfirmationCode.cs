using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class ResendConfirmationCode : EntityBase
    {
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<ConfirmSignUp>()
        {
            return new ResendConfirmationCodeValidator().Validate(this);
        }
    }
}