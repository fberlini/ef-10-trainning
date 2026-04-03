using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Api.Data;
using MiniBlog.Api.WebApi.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextConfiguration(builder.Configuration);
builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<ITagQueries, TagQueries>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/users", async ([FromQuery] bool? asTracking, IUserQueries userQuery) =>
{
    var users = asTracking == true ? await userQuery.GetAllUsersTrackingAsync() : await userQuery.GetAllUsersAsNoTrackingAsync();
    return users;

})
.WithName("GetAllUsers");

app.MapGet("/users/{id}", async ([FromRoute] Guid id, IUserQueries userQuery) =>
{
    var user = await userQuery.GetUserByIdAsync(id);
    return user != null ? Results.Ok(user) : Results.NotFound();

})
.WithName("GetUserById");

app.MapGet("/users/posts", async ([FromQuery] Guid? lastUserId, IUserQueries userQuery) =>
{
    var users = await userQuery.GetUsersWithPostsAsync(lastUserId);
    return users;

})
.WithName("GetUsersWithPosts");

app.MapGet("/users/{id}/profile", async ([FromRoute] Guid id, IUserQueries userQuery) =>
{
    var userProfile = await userQuery.GetUserProfilePageAsync(id);
    return userProfile != null ? Results.Ok(userProfile) : Results.NotFound();

})
.WithName("GetUserProfilePage");

app.MapGet("/users/tags/aggregation", async ([FromQuery] Guid? lastUserId, IUserQueries userQuery) =>
{
    var usersWithTagsAggregation = await userQuery.GetUsersWithTagsAggregationAsync(lastUserId);
    return usersWithTagsAggregation;

}).WithName("GetUsersWithTagsAggregation");

app.MapGet("/tags/most-used", async ([FromQuery] int topN, ITagQueries tagQuery) =>
{
    var mostUsedTags = await tagQuery.GetMostUsedTagsAsync(topN);
    return mostUsedTags;

}).WithName("GetMostUsedTags");

app.Run();
