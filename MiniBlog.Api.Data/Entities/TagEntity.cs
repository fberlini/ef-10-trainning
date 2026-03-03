namespace MiniBlog.Api.Data.Entities;

public class TagEntity
{
    public required Guid TagId { get; set; }
    public required string Name { get; set; }

    public required List<PostEntity> Posts { get; set; }
}