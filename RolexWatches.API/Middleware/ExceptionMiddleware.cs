using Microsoft.Data.SqlClient;
using RolexWatches.Application.Common;
using System.Net;
using System.Text.Json;

namespace RolexWatches.API.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                int status;
                string message;

                if (IsDatabaseUnavailable(ex))
                {
                    status = (int)HttpStatusCode.ServiceUnavailable;
                    message = "The service is temporarily unavailable. Please try again in a moment.";
                }
                else
                {
                    switch (ex)
                    {
                        case UnauthorizedAccessException:
                            status = (int)HttpStatusCode.Unauthorized;
                            message = ex.Message;
                            break;
                        case InvalidOperationException:
                            status = (int)HttpStatusCode.BadRequest;
                            message = ex.Message;
                            break;
                        case KeyNotFoundException:
                            status = (int)HttpStatusCode.NotFound;
                            message = ex.Message;
                            break;
                        default:
                            status = (int)HttpStatusCode.InternalServerError;
                            message = "Something went wrong. Please try again.";
                            break;
                    }
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = status;

                var response = ApiResponse<object>.FailResponse(message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
            }
        }

        // Walks the inner exceptions, because EF wraps the SqlException
        private static bool IsDatabaseUnavailable(Exception? ex)
        {
            while (ex != null)
            {
                if (ex is SqlException || ex is TimeoutException)
                    return true;
                ex = ex.InnerException;
            }
            return false;
        }
    }
}