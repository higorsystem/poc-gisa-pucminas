namespace GISA.Authentication.Application.ViewModels
{
    public class ChangeEmailRequestViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}