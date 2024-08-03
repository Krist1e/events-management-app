namespace EventsManagementApp.Application.Common.Contracts;

public record PaginationMetadata
{
    public PaginationMetadata(int totalCount, int currentPage, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
    }

    public int TotalCount { get; }
    public int PageSize { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
}