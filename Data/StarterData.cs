using Backend_VideoGamesCatalogue.Data;
using Backend_VideoGamesCatalogue.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend_VideoGamesCatalogue.Data
{
    public class StarterData
    {
        public static async Task EnsureSeedDataAsync(AppDBContext db, ILogger logger)
        {
            // Only seed if no rows
            if (!await db.VideoGames.AnyAsync())
            {
                logger.LogInformation("Seeding initial VideoGames data.");

                var videoGames = new List<VideoGame>
                                {
                                    new VideoGame { Title = "Bomb Rush Cyberfunk", Genre = "Adventure", Platform = "Playstation 5(PS5)", Price = 49.99m, ReleaseDate = new DateOnly(2012, 10,1), ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmOQkcoh1zmOQyHTGnPXbyQbnyRmf5puzQvw&s" }
                                    , new VideoGame { Title = "Carrion", Genre = "Horror", Platform = "Switch", Price = 39.99m, ReleaseDate = new DateOnly(2021, 1,10), ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202107/0620/NH2ucTdNQgwnaACWmxo4iAPF.png" }
                                    , new VideoGame { Title = "Signalis", Genre = "Sci-fi", Platform = "Playstation 4(PS4)", Price = 29.99m, ReleaseDate = new DateOnly(2009, 3,15), ImageUrl = "https://www.humblegames.com/wp-content/uploads/2021/06/SIGNALIS-Combat-6.png" },
                                };

                await db.VideoGames.AddRangeAsync(videoGames);
                await db.SaveChangesAsync();

                logger.LogInformation("Video games seeding completed.");
            }
            else
            {
                logger.LogInformation("Video games already exist.");
            }
        }
    }
}
