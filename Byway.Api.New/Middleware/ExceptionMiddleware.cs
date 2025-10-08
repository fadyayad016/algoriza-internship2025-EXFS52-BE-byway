using Byway.Api.New.Errors;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Byway.Api.New.Middleware
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
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        apiExceptionResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                        apiExceptionResponse.Message = "Unauthorized Access";
                        apiExceptionResponse.Details = ex.Message;
                        break;

                    case ValidationException validationEx:
                        // This handles FluentValidation errors specifically
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        apiExceptionResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        apiExceptionResponse.Message = "Validation Error";

                        apiExceptionResponse.Details = validationEx.Errors != null && validationEx.Errors.Any()
                                                   ? JsonSerializer.Serialize(validationEx.Errors.Select(e => e.ErrorMessage))
                                                   : ex.Message; break;
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


            if (!context.Response.HasStarted)
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    context.Response.ContentType = "application/json";
                    var apiResponse = new ApiExceptionResponse
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden,
                        Message = "Access Denied: You do not have permission to perform this action."
                    };
                    var jsonResponse = JsonSerializer.Serialize(apiResponse);
                    await context.Response.WriteAsync(jsonResponse);
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.ContentType = "application/json";
                    var apiResponse = new ApiExceptionResponse
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = "Authentication Failed: A valid token is required."
                    };
                    var jsonResponse = JsonSerializer.Serialize(apiResponse);
                    await context.Response.WriteAsync(jsonResponse);
                }

            }
        }
    }
}
