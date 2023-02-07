using Microsoft.Extensions.Options;
using NewsBook.Data;
using NewsBook.Repository;
using System.Security.Claims;

namespace NewsBook.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettingsDTO _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettingsDTO> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUsersRepository userRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = await userRepository.GetById(userId.Value);     
            }
            await _next(context);
        }
    }
}
