namespace EventManagementApp.Contracts.Users;

public record UserResponse(
    long Id,
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);