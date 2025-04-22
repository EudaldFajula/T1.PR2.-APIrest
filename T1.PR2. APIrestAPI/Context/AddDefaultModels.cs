using Microsoft.EntityFrameworkCore;
using T1.PR2._APIrestAPI.Models;

namespace T1.PR2._APIrestAPI.Context
{
    public static class AddDefaultModels
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Title = "Celeste",
                    Description = "Celeste takes place on a fictional version of Celeste Mountain containing several areas. It is seemingly haunted, housing many strange occurrences. The protagonist, a young woman named Madeline, decides to climb the mountain to challenge her anxiety, stubbornly persisting until she reaches its summit.",
                    DeveloperTeam = "Maddy Makes Games",
                    ImgUrl = "https://static.gosugamers.net/f8/a0/01/00a04819cfa294a5e0a3cb2706fc967fc81dab171249a8bf94ca0711ea.webp?w=1600"
                },
                new Game
                {
                    Id = 2,
                    Title = "Disco Elysium",
                    Description = "Disco Elysium follows a troubled detective waking up from a hangover with no memory of who he is, or of the world around him. As he investigates a murder alongside a detective from another precinct, the player can also piece together the protagonist's own identity and discover what led him to this state.",
                    DeveloperTeam = "ZA/UM",
                    ImgUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202102/1011/mjNseThhuPx9VS6h8MmbtHRG.png"
                },
                new Game
                {
                    Id = 3,
                    Title = "Katana Zero",
                    Description = "Katana ZERO is a stylish neo-noir, action-platformer featuring breakneck action and instant-death combat. Slash, dash, and manipulate time to unravel your past in a beautifully brutal acrobatic display.",
                    DeveloperTeam = "Askiisoft",
                    ImgUrl = "https://www.nintendo.com/eu/media/images/10_share_images/games_15/nintendo_switch_download_software_1/H2x1_NSwitchDS_KatanaZero_image1600w.jpg"
                },
                new Game
                {
                    Id = 4,
                    Title = "Metal Gear Solid 3",
                    Description = "You reprise your role as Solid Snake, an elite tactical soldier. This time, you are sent on a dangerous mission to both infiltrate enemy territory and uncover the secrets behind a newly created weapon of mass destruction.",
                    DeveloperTeam = "Kojima Productions",
                    ImgUrl = "https://gaming-cdn.com/images/news/articles/9190/cover/konami-wollte-ein-remake-von-metal-gear-solid-3-erstellen-um-jungen-spielern-die-moglichkeit-zu-geben-die-saga-zu-entdecken-cover6730c1ad9b420.jpg"
                },
                new Game
                {
                    Id = 5,
                    Title = "Metal Gear Solid 4",
                    Description = "Amassing an army whose manpower rivals that of the United States, Liquid prepares to launch an armed insurrection. With the world once again in crisis, a rapidly aging and disillusioned Solid Snake is deployed into the Middle East by Roy Campbell to terminate Liquid.",
                    DeveloperTeam = "Kojima Productions",
                    ImgUrl = "https://www.play3.de/wp-content/uploads/2024/08/Metal-Gear-Solid-4-Guns-of-the-Patriots.jpg"
                }
                
            );
        }
    }
}
