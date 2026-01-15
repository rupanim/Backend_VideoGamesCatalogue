
using System.Net;
using Backend_VideoGamesCatalogue.Data;
using Backend_VideoGamesCatalogue.Model;
using Backend_VideoGamesCatalogue.Endpoints;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend_VideoGamesCatalogue.Tests
{

    public class VideoGamesEndpointsTests : IClassFixture<TestAppFactory>
    {
        private readonly HttpClient _client;
        private readonly IServiceProvider _services;

        public VideoGamesEndpointsTests(TestAppFactory factory)
        {
            _client = factory.CreateClient();
            _services = factory.Services;
        }

        private async Task<int> SeedAsync(string title, decimal price, string platform, string genre, DateOnly releaseDate, string imageUrl)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var g = new VideoGame { 
                Title = title, 
                Price = price, 
                Platform = platform, 
                Genre = genre, 
                ReleaseDate = releaseDate, 
                ImageUrl = imageUrl
            };
            db.VideoGames.Add(g);
            await db.SaveChangesAsync();
            return g.Id;
        }

        [Fact]
        public async Task Get_ById()
        {
            var id = await SeedAsync("Grand Theft Auto", 70m, "Xbox", "Shooter", new DateOnly(2021, 11, 15), "https://example.com/test.jpg");

            var res = await _client.GetAsync($"/api/videogames/{id}");
            res.EnsureSuccessStatusCode();

            var item = await res.Content.ReadFromJsonAsync<VideoGame>();
            item!.Title.Should().Be("Grand Theft Auto");
        }

        [Fact]
        public async Task Get_All_Returns_All_Items()
        {
            // Arrange – seed multiple items
            await SeedAsync("Abiotic Factor", 70m, "Xbox", "Shooter", new DateOnly(2009, 11, 15), "https://example.com/test1.jpg");
            await SeedAsync("FIFA 13", 60m, "PlayStation", "Sports", new DateOnly(2010, 11, 15), "https://example.com/test2.jpg");
            await SeedAsync("Halo", 50m, "Xbox", "Shooter", new DateOnly(2011, 11, 15), "https://example.com/test3.jpg");

            // Act
            var res = await _client.GetAsync("/api/videogames");
            res.EnsureSuccessStatusCode();

            var items = await res.Content.ReadFromJsonAsync<List<VideoGame>>();

            // Assert
            items.Should().NotBeNull();

            items.Select(x => x.Title).Should().Contain(new[] 
                {
                "Abiotic Factor",
                "FIFA 13",
                "Halo"
                });
        }

        [Fact]
        public async Task Put_ById_Updates_VideoGame()
        {
            // Arrange – seed initial entity
            var id = await SeedAsync(
                title: "Battlefield",
                price: 70m,
                platform: "Xbox",
                genre: "Shooter",
                releaseDate: new DateOnly(2012, 11, 15),
                imageUrl: "https://example.com/test1.jpg"
            );

            var updatedGame = new VideoGame
            {
                Id = id, // optional, route id is authoritative
                Title = "Battlefield Updated",
                Price = 80m,
                Platform = "PlayStation",
                Genre = "Action",
                ReleaseDate = new DateOnly(2024, 1, 1),
                ImageUrl = "https://example.com/test2.jpg"
            };

            // Act
            var res = await _client.PutAsJsonAsync($"/api/videogames/{id}", updatedGame);
            res.EnsureSuccessStatusCode();

            var returned = await res.Content.ReadFromJsonAsync<VideoGame>();

            // Assert – response
            returned.Should().NotBeNull();
            returned!.Title.Should().Be("Battlefield Updated");
            returned.Price.Should().Be(80m);
            returned.Platform.Should().Be("PlayStation");
            returned.Genre.Should().Be("Action");

            // Assert – persisted data (important)
            var getRes = await _client.GetAsync($"/api/videogames/{id}");
            getRes.EnsureSuccessStatusCode();

            var persisted = await getRes.Content.ReadFromJsonAsync<VideoGame>();
            persisted!.Title.Should().Be("Battlefield Updated");
            persisted.Price.Should().Be(80m);
        }
    }
}


