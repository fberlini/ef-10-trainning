namespace MiniBlog.Api.WebApi.Dtos;

public class GetMostUsedTagsResponse
{
    public required Guid TagId { get; set; }
    public required string Name { get; set; }
    public required int PostsCount { get; set; }
    public required IReadOnlyList<TopUsersDto> TopUsers { get; set; }
}

public class TopUsersDto
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required int PostsCount { get; set; }
}