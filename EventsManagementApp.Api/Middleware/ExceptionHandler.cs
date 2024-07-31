using System.Net;
using EventsManagementApp.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagementApp.Middleware;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorCode = exception switch
        {
            AddImagesFailedException => HttpStatusCode.BadRequest,
            EventNotFoundException => HttpStatusCode.NotFound,
            ImagesNotFoundException => HttpStatusCode.NotFound,
            LoginFailedException => HttpStatusCode.BadRequest,
            RefreshTokenException => HttpStatusCode.BadRequest,
            RegisterFailedException => HttpStatusCode.BadRequest,
            RegisterInEventFailedException => HttpStatusCode.BadRequest,
            RemoveImagesFailedException => HttpStatusCode.InternalServerError,
            RoleAssignmentFailedException => HttpStatusCode.BadRequest,
            UnregisterFromEventFailedException => HttpStatusCode.BadRequest,
            UserNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError,
        };
        
        var response = new ProblemDetails
        {
            Title = exception.Message,
            Status = (int) errorCode
        };
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}