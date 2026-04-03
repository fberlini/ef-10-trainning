using MiniBlog.Api.Data;
using MiniBlog.Api.WebApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MiniBlog.Api.WebApi.Queries;

public interface ITagQueries
{
    Task<IReadOnlyList<GetMostUsedTagsResponse>> GetMostUsedTagsAsync(int topN);
}

public class TagQueries(MiniBlogDbContext dbContext) : ITagQueries
{
    private readonly MiniBlogDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<GetMostUsedTagsResponse>> GetMostUsedTagsAsync(int topN)
    {
        return await _dbContext.Tags
            .Select(t => new GetMostUsedTagsResponse
            {
                TagId = t.TagId,
                Name = t.Name,
                PostsCount = t.Posts.Count,
                TopUsers = t.Posts
                    .Select(p => p.Author)
                    .GroupBy(u => u.UserId)
                    .Select(g => new TopUsersDto
                    {
                        UserId = g.Key,
                        Username = g.First().Username,
                        PostsCount = g.Count()
                    })
                    .OrderByDescending(u => u.PostsCount)
                    .Take(5)
                    .ToList()
            })
            .OrderByDescending(t => t.PostsCount)
            .Take(topN)
            .ToListAsync();
    }
}