using System.Net;
using System.Text.Json;
using EventsManagementApp.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagementApp.Middleware;

public class ExceptionHandler : IExceptionHandler
{
    private const string ServerErrorMessage = "An error occurred";

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
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
        
        var response = new ProblemDetails
        {
            Title = errorCode != HttpStatusCode.InternalServerError ? exception.Message : ServerErrorMessage,
            Status = (int) errorCode
        };
        
        httpContext.Response.StatusCode = (int) errorCode;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}