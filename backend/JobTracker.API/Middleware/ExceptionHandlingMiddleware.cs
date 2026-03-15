
using JobTracker.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace JobTracker.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public ExceptionHandlingMiddleware(ILogger logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound, 
                ValidationException => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Forbidden, 
                _ => HttpStatusCode.InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = GetTitle(statusCode),
                Detail = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Please try again later."
                    : exception.Message,
                Type = $"https://tools.ietf.org/html/rfc7231#section-6.{(int)statusCode / 100}.{(int)statusCode % 100}"
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }

        private static string GetTitle(HttpStatusCode statusCode) => statusCode switch
        {
            HttpStatusCode.NotFound => "Not Found",
            HttpStatusCode.BadRequest => "Bad Request",
            HttpStatusCode.Forbidden => "Forbidden",
            HttpStatusCode.InternalServerError => "Internal Server Error",
            _ => "Error"
        };
    }
}
