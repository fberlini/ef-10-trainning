namespace MiniBlog.Api.WebApi.Dtos;

public class GetUsersWithPostsResponse
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required List<PostDto> Posts { get; set; }
}

public class PostDto
{
    public required Guid PostId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }
}