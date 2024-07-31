namespace EventsManagementApp.Application.Common.Exceptions;

public class RoleAssignmentFailedException : Exception
{
    public RoleAssignmentFailedException(string message) : base(message)
    {
    }
}