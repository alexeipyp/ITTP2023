using Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserNotFoundException exception)
            {
                await HandleException(context, exception, HttpStatusCode.NotFound);
            }
            catch (UnauthorizedException exception)
            {
                await HandleException(context, exception, HttpStatusCode.Unauthorized);
            }
            catch (OnlyAdminsException exception)
            {
                await HandleException(context, exception, HttpStatusCode.Forbidden);
            }
            catch (NotUniqueLoginException exception)
            {
                await HandleException(context, exception, HttpStatusCode.BadRequest);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception, HttpStatusCode code)
        {
            string result;
            if (exception.InnerException != null)
            {
                result = JsonSerializer.Serialize(new
                {
                    Error = exception.Message,
                    ExceptionType = exception.GetType().FullName,
                    InnerExceptionValue = exception.InnerException.Message,
                    InnerExceptionType = exception.InnerException.GetType().FullName,
                });
            }
            else
            {
                result = JsonSerializer.Serialize(new
                {
                    Error = exception.Message,
                    ExceptionType = exception.GetType().FullName,
                });
            }
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtentions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

}
