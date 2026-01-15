
// Endpoints/VideoGamesEndpoints.cs
using Backend_VideoGamesCatalogue.Data;
using Backend_VideoGamesCatalogue.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend_VideoGamesCatalogue.Endpoints;

public static class VideoGamesEndpoints
{
    public static IEndpointRouteBuilder MapVideoGameEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/videogames", async (AppDBContext db) =>
            await db.VideoGames.AsNoTracking().OrderByDescending(game => game.Id).ToListAsync());

        app.MapGet("/api/videogames/{id:int}", async (int id, AppDBContext db) =>
        {
            var videoGame = await db.VideoGames.FindAsync(id);
            return videoGame is not null ? Results.Ok(videoGame) : Results.NotFound();
        });

        app.MapPut("/api/videogames/{id:int}", async (int id, VideoGame input, AppDBContext db) =>
        {
            var videoGame = await db.VideoGames.FindAsync(id);
            if (videoGame is null) return Results.NotFound();

            videoGame.Title = input.Title;
            videoGame.Price = input.Price;
            videoGame.Platform = input.Platform;
            videoGame.ReleaseDate = input.ReleaseDate;
            videoGame.ImageUrl = input.ImageUrl;
            videoGame.Genre = input.Genre;

            await db.SaveChangesAsync();
            return Results.Ok(videoGame);
        });

        return app;
    }
}
