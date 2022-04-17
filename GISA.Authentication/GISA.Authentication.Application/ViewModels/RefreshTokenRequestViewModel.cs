namespace GISA.Authentication.Application.ViewModels
{
    public class LoginRequestViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}