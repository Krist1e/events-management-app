namespace EventsManagementApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync(CancellationToken cancellationToken);
    Task RollbackChangesAsync(CancellationToken cancellationToken);
    
    IEventRepository EventRepository { get; }
    IImageRepository ImageRepository { get; }
    IRoleRepository RoleRepository { get; }
    IUserRepository UserRepository { get; }
}