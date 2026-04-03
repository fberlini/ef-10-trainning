using Microsoft.EntityFrameworkCore;
using MiniBlog.Api.Data;
using MiniBlog.Api.Data.Entities;
using MiniBlog.Api.WebApi.Dtos;

namespace MiniBlog.Api.WebApi.Queries;

public interface IUserQueries
{
    // Write
    Task AddUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity user);

    // Read
    Task<UserEntity?> GetUserByIdAsync(Guid id);
    Task<IReadOnlyList<UserEntity>> GetAllUsersTrackingAsync();
    Task<IReadOnlyList<UserEntity>> GetAllUsersAsNoTrackingAsync();
    Task<IReadOnlyList<GetUsersWithPostsResponse>> GetUsersWithPostsAsync(Guid? lastUserId);
    Task<UserEntity?> GetUserProfilePageAsync(Guid userId);
    Task<IReadOnlyList<GetUsersWithTagsAggregationResponse>> GetUsersWithTagsAggregationAsync(Guid? lastUserId);
}

public class UserQueries(MiniBlogDbContext dbContext) : IUserQueries
{
    private readonly MiniBlogDbContext _dbContext = dbContext;

    public async Task AddUserAsync(UserEntity user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user;
    }

    // AsNoTracking() is used to improve performance when you don't need to track changes to the entities.
    public async Task<IReadOnlyList<UserEntity>> GetAllUsersAsNoTrackingAsync()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    // This method will track changes to the entities, which can be useful if you plan to update them later.
    public async Task<IReadOnlyList<UserEntity>> GetAllUsersTrackingAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<IReadOnlyList<GetUsersWithPostsResponse>> GetUsersWithPostsAsync(Guid? lastUserId)
    {
        return await _dbContext.Users
            .OrderBy(u => u.CreatedAt)
            .ThenBy(u => u.UserId)
            .Where(u => !lastUserId.HasValue || u.UserId > lastUserId.Value)
            .Take(10)
            .Include(u => u.Posts)
            .AsNoTracking()
            .Select(u => new GetUsersWithPostsResponse
            {
                UserId = u.UserId,
                Username = u.Username,
                Posts = u.Posts.Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<UserEntity?> GetUserProfilePageAsync(Guid userId)
    {
        return await _dbContext.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.Posts)
            .ThenInclude(p => p.Tags)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<GetUsersWithTagsAggregationResponse>> GetUsersWithTagsAggregationAsync(Guid? lastUserId)
    {
        return await _dbContext.Users
            .OrderBy(u => u.CreatedAt)
            .ThenBy(u => u.UserId)
            .Where(u => !lastUserId.HasValue || u.UserId > lastUserId.Value)
            .Take(10)
            .Include(u => u.Posts)
            .ThenInclude(p => p.Tags)
            .AsNoTracking()
            .Select(u => new GetUsersWithTagsAggregationResponse
            {
                UserId = u.UserId,
                Username = u.Username,
                Tags = u.Posts
                    .SelectMany(p => p.Tags)
                    .GroupBy(t => t.TagId)
                    .Select(g => new TagsAggegation
                    {
                        TagId = g.Key,
                        Name = g.First().Name,
                        PostsCount = g.Count()
                    })
                    .OrderByDescending(t => t.PostsCount)
                    .Take(5)
                    .ToList()
            })
            .ToListAsync();
    }
}
