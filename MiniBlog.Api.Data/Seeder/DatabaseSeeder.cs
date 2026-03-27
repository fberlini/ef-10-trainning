using Bogus;
using Bogus.Extensions;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data.Seeder;

public static class DatabaseSeeder
{
    public static void Seed(MiniBlogDbContext context)
    {
        Console.WriteLine("Starting database seeding...");

        ArgumentNullException.ThrowIfNull(context);

        var listUsers = SeedUsers(context);
        var listTags = SeedTags(context);
        SeedPosts(context, listUsers, listTags);
    }

    private static List<UserEntity> SeedUsers(MiniBlogDbContext context)
    {
        Console.WriteLine("Seeding users...");
        
        if (context.Users.Any())
        {
            Console.WriteLine("Users already exist. Skipping seeding for users.");
            return [..context.Users];
        }

        var users = new Faker<UserEntity>()
            .RuleFor(x => x.Username, f => f.Person.UserName.ClampLength(1, 50))
            .RuleFor(x => x.Email, f => f.Person.Email.ClampLength(1, 100))
            .Generate(5000);

        context.Users.AddRange(users);
        context.SaveChanges();

        Console.WriteLine("Users seeding completed.");
        return users;
    }

    private static List<TagEntity> SeedTags(MiniBlogDbContext context)
    {
        Console.WriteLine("Seeding tags...");

        if (context.Tags.Any())
        {
            Console.WriteLine("Tags already exist. Skipping seeding for tags.");
            return [..context.Tags];
        }

        var tags = new Faker<TagEntity>()
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .Generate(100);

        context.Tags.AddRange(tags);
        context.SaveChanges();

        Console.WriteLine("Tags seeding completed.");
        return tags;
    }

    private static void SeedPosts(MiniBlogDbContext context, List<UserEntity> users, List<TagEntity> tags)
    {
        Console.WriteLine("Seeding posts...");

        if (context.Posts.Any())
        {
            Console.WriteLine("Posts already exist. Skipping seeding for posts.");
            return;
        }

        var posts = new Faker<PostEntity>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Content, f => f.Lorem.Paragraphs(3))
            .RuleFor(x => x.AuthorId, f => f.PickRandom(users).UserId)
            .RuleFor(x => x.Tags, f => [.. f.PickRandom(tags, f.Random.Int(1, 5))])
            .Generate(1000000);

        context.Posts.AddRange(posts);
        context.SaveChanges();

        Console.WriteLine("Posts seeding completed.");
    }
}
