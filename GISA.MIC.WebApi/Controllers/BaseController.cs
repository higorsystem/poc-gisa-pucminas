using System;
using System.Linq;
using System.Threading.Tasks;
using GISA.Commons.SDK.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace GISA.MIC.WebApi.Controllers
{
    /// <inheritdoc />
    public class BaseController : Controller
    {
        private readonly IDistributedCache _cache;
       
        private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions();

        /// <inheritdoc />
        public BaseController(IDistributedCache cache)
        {
            _cache = cache;
            _cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        }

        /// <summary />
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetCacheData(string key)
        {
            return _cache.GetString(key);
        }

        /// <summary />
        protected long PersonId
        {
            get
            {
                var claimResult = User?
                    .Claims?
                    .FirstOrDefault(c => c.Type == EClaimType.Person.ToDescription())?
                    .Value;

                return Convert.ToInt64(claimResult);
            }
        }

        /// <summary />
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected async Task SetCacheDataAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value, _cacheOptions);
        }
    }
}