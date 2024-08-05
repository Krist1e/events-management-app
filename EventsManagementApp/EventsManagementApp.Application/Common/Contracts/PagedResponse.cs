namespace EventsManagementApp.Application.Common.Contracts;

public record PagedResponse<T>(IEnumerable<T> Items, PaginationMetadata? Metadata);