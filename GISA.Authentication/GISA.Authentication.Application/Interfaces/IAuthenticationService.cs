using System.Threading.Tasks;
using GISA.Authentication.Application.ViewModels;

namespace GISA.Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseViewModel> SignUpAsync(SignUpRequestViewModel signUpViewModel);
        Task<bool> SignOutAsync(SignOutRequestViewModel signOutViewModel);
        Task<bool> ConfirmSignUpAsync(ConfirmSignUpRequestViewModel confirmSignUpViewModel);
        Task<LoginResponseViewModel> LoginAsync(LoginRequestViewModel loginViewModel);
        Task<LoginResponseViewModel> LoginJwtTokenChangedAsync(LoginRequestViewModel loginViewModel);        
        Task<LoginResponseViewModel> RefreshTokenAsync(RefreshTokenRequestViewModel refreshTokenViewModel);
        Task<bool> ResendConfirmationCodeAsync(ResendConfirmationCodeRequestViewModel resendConfirmationCodeViewModel);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestViewModel forgotPasswordViewModel);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestViewModel resetPasswordRequestViewModel);
        Task<bool> ChangePasswordAsync(ChangePasswordRequestViewModel changePasswordRequestViewModel);
        Task<bool> ChangeEmailAsync(ChangeEmailRequestViewModel changeEmailRequestViewModel);
    }
}