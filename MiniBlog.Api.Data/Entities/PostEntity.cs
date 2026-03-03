namespace MiniBlog.Api.Data.Entities;

public class PostEntity
{
    public required Guid PostId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required Guid AuthorId { get; set; }

    public required UserEntity Author { get; set; }
    public required List<TagEntity> Tags { get; set; }
}