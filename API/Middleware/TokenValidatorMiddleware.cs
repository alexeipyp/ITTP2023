using BLL.Services;
using Common.Consts;
using Common.Exceptions;
using Common.Extentions;
using System.Net;

namespace API.Middleware
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthService authService)
        {
            var isOk = true;
            var userGuid = context.User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            if (userGuid != default)
            {
                if (!(await authService.IsUserActive(userGuid)))
                {
                    throw new UnauthorizedException();
                }
            }
            if (isOk)
            {
                await _next(context);
            }
        }
    }

    public static class TokenValidatorMiddlewareExtentions
    {
        public static IApplicationBuilder UseTokenValidator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidatorMiddleware>();
        }
    }
}
