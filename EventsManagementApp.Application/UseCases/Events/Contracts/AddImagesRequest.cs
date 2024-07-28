using Microsoft.AspNetCore.Http;

namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record AddImagesRequest(IFormFileCollection ImageFiles);