namespace MiniBlog.Api.WebApi.Dtos;

public class GetUsersWithTagsAggregationResponse
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required IReadOnlyList<TagsAggegation> Tags { get; set; }
}

public class TagsAggegation
{
    public required Guid TagId { get; set; }
    public required string Name { get; set; }
    public required int PostsCount { get; set; }
}