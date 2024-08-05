using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace EventsManagementApp.Application.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(RefreshRequest Refresh) : IRequest<ClaimsPrincipal>;