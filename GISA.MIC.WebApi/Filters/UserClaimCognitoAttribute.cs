using System;
using System.Linq;
using System.Threading.Tasks;
using GISA.Commons.SDK.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GISA.MIC.WebApi.Filters
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UserClaimCognitoAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary />
        public string UserTypeValue { get; set; }

        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userTypeValue = context
                .HttpContext
                .User?
                .Claims?
                .FirstOrDefault(c => c.Type == EClaimType.UserType.ToDescription())?
                .Value;

            if (string.IsNullOrEmpty(userTypeValue) || !userTypeValue.Equals(UserTypeValue))
            {
                context.Result = new UnauthorizedObjectResult("401 - Unauthorized requisition.");
                return;
            }

            await next();
        }
    }
}