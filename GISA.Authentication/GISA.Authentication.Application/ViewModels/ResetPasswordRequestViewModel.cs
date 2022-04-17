namespace GISA.Authentication.Application.ViewModels
{
    public class ResetPasswordRequestViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}