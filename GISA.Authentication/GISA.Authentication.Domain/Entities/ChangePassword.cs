using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class ChangePassword : EntityBase
    {
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<ChangePassword>()
        {
            return new ChangePasswordValidator().Validate(this);
        }
    }
}