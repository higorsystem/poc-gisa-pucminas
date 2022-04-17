using GISA.Authentication.Application.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace GISA.Authentication.WebApi.Controllers
{
    [Produces("application/json")]
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly NotificationContext _notificationContext;

        protected ApiBaseController(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        protected ActionResult CustomResponse(ActionResult actionResult)
        {
            return !_notificationContext.HasNotifications ? actionResult : BadRequest(_notificationContext.Notifications);
        }

        protected ActionResult CustomResponse()
        {
            return CustomResponse(Ok());
        }
    }
}