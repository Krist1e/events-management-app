using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Enums;
using EventsManagementApp.Application.UseCases.Events.Contracts;

namespace EventsManagementApp.Infrastructure.Common.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<Event> Filter(this IQueryable<Event> query, EventQueryParameters queryParameters)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.Category))
        {
            query = query.Where(e => e.Category.Equals(Enum.Parse<CategoryEnum>(queryParameters.Category, true)));
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.Location))
        {
            query = query.Where(e => e.Location.Contains(queryParameters.Location));
        }

        return query;
    }

    public static IQueryable<Event> Search(this IQueryable<Event> query, EventQueryParameters queryParameters)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.Name))
        {
            query = query.Where(e => e.Name.Contains(queryParameters.Name));
        }

        if (queryParameters.StartDate.HasValue)
        {
            query = query.Where(e => e.StartDate >= queryParameters.StartDate);
        }

        if (queryParameters.EndDate.HasValue)
        {
            query = query.Where(e => e.EndDate <= queryParameters.EndDate);
        }

        return query;
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, QueryParameters queryParameters)
    {
        return query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);
    }

    public static IQueryable<Event> Sort(this IQueryable<Event> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            return query;
        }

        var order = orderBy.Split(' ');
        var sortOrder = order.Length > 1 && order[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase)
            ? SortOrder.Descending
            : SortOrder.Ascending;

        var sortParameter = Enum.Parse<SortParameter>(order[0], true);

        return sortOrder switch
        {
            SortOrder.Ascending => sortParameter switch
            {
                SortParameter.StartDate => query.OrderBy(e => e.StartDate),
                SortParameter.EndDate => query.OrderBy(e => e.EndDate),
                _ => query
            },
            SortOrder.Descending => sortParameter switch
            {
                SortParameter.StartDate => query.OrderByDescending(e => e.StartDate),
                SortParameter.EndDate => query.OrderByDescending(e => e.EndDate),
                _ => query
            },
            _ => throw new ArgumentOutOfRangeException(message: "Invalid sort order", null)
        };
    }
}