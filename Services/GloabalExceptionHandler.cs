using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagment.Exceptions;

namespace UserManagment.Services
{
    public class GloabalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = exception switch
            {
                ServerException serverException => serverException.StatusCode,
                _ => StatusCodes.Status500InternalServerError,
            };
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Type = exception.GetType().Name,
                    Title = "An error occured",
                    Detail = exception is ServerException ? exception.Message : "Inner exception"
                }
            });
        }
    }
}
