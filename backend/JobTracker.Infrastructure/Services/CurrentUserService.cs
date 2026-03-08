using JobTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                // ClaimTypes.NameIdentifier is the standard claim where
                // ASP.NET Identity stores the User's ID inside the JWT
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                // If no token / not logged in, value will be null
                // If logged in, parse the string claim value into a Guid
                if(Guid.TryParse(value, out var id))
                {
                    return id;
                }
                return Guid.Empty;
            }
        }
    }
}
