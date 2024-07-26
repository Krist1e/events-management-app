namespace EventsManagementApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync(CancellationToken cancellationToken);
    Task RollbackChangesAsync(CancellationToken cancellationToken);
}