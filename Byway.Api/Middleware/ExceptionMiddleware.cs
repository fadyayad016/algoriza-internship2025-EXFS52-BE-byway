using Byway.Api.Errors;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Byway.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // This passes the request to the next component in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // If an exception occurs, we log it and handle it
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                var response = context.Response;

                var apiExceptionResponse = new ApiExceptionResponse();

                switch (ex)
                {
                    case ValidationException validationEx:
                        // This handles FluentValidation errors specifically
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        apiExceptionResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        apiExceptionResponse.Message = "Validation Error";
                        apiExceptionResponse.Details = JsonSerializer.Serialize(validationEx.Errors.Select(e => e.ErrorMessage));
                        break;
                    default:
                        // This handles all other unhandled errors
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        apiExceptionResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        apiExceptionResponse.Message = "An internal server error has occurred.";
                        apiExceptionResponse.Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null;
                        break;
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonResponse = JsonSerializer.Serialize(apiExceptionResponse, options);
                await context.Response.WriteAsync(jsonResponse);
            }

        }
    }
}
