namespace MiniBlog.Api.Data.Entities;

public class UserEntity
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTime CreatedAt { get; set; }
}