using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using GISA.MIC.Application.Helper;
using Google.Rpc.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace GISA.MIC.Application.Service.Handler
{
    public abstract class ServiceBaseHandler
    {
        private readonly UserManager<CognitoUser> _userManager;

        protected ServiceBaseHandler(UserManager<CognitoUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary />
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetClaimByEmailAsync(string email)
        {
            var userFound = await _userManager.FindByEmailAsync(email);
            return await _userManager.GetClaimsAsync(userFound);
        }

        public Task<Response<T>> ReturnResponseMessageAsync<T>(T response, string message = default, bool isSuccess = default, HttpStatusCode statusCode = default)
        {
            return Task.FromResult(new Response<T>(response, message, isSuccess, statusCode));
        }
    }
}
