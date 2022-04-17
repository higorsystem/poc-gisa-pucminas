using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class ChangeEmail : EntityBase
    {
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<ChangeEmail>()
        {
            return new ChangeEmailValidator().Validate(this);
        }
    }
}