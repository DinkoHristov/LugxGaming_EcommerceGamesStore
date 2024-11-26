using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Enums;
using LugxGaming.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LugxGaming.Data
{
    public class DataSeed
    {
        public static async Task AddAdmin(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Adding roles to the db
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Create admin user
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FirstName = "admin",
                LastName = "admin"
            };

            var foundUser = await userManager.FindByEmailAsync(admin.Email);

            if (foundUser == null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        public static async Task AddGenres(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Genres.AnyAsync())
            {
                await dbContext.Genres.AddRangeAsync(new[]
                {
                    new Genre{ Name = "Action" },
                    new Genre{ Name = "Adventure" },
                    new Genre{ Name = "Strategy" },
                    new Genre{ Name = "Racing" },
                    new Genre{ Name = "Sport" }
                });

                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task AddGames(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Games.AnyAsync())
            {
                await dbContext.Games.AddRangeAsync(new[]
                {
                    new Game() { Name = "Fallout 4", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/ch99eH3EBogFypt3mKGwsk-1200-80.jpg.webp", Price = 19.99m, Description = "Fallout 4 may be bleak at times, what with the whole nuclear devastation and that, but it ultimately presents us with lands to explore that are rich in detail, populated by unforgettable characters - robot detectives! Ghoulified radio lovers! -, and ripe for the chance to build your own community on. While you wander through the scorched remains of a society devastated by an invisible enemy (in the form of radiation)." },
                    new Game() { Name = "A Way Out", GenreId = 2, ImageUrl = "https://cdn.mos.cms.futurecdn.net/75oM9gEUohayDJDPFiGzKn-1200-80.jpg.webp", Price = 39.99m, Description = "It’s an inventive set-up, in which players must work with one another smartly if they are to make any kind of progress - at some point, one player will have to create a distraction so that the other can pick up an object that will aid them in their escape plan. By the end of the game, the two players helping the protagonists Leo and Vincent to go on the run will become as close as the characters they’re controlling and may even shed a tear or two as the story nears its conclusion." },
                    new Game() { Name = "Starfield", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/ijJQk59d3QoDR68S7RNc8U-1200-80.jpg.webp", Price = 69.99m, Description = "If exploring the galaxy is your idea of a good time, you've probably at least heard of Starfield already. Bethesda's sprawling deep space RPG is a lot more than an open world adventure: it's an intergalactic one, and that means the sky's no limit at all. With skip customization, a range of unique character backgrounds to choose from, and of course no shortage of winding journeys to take among the stars, Starfield is a veritable adventure playground." },
                    new Game() { Name = "Ratchet & Clank: Rift Apart", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/zjgAMz5Cpe4SD5ABG2gp9n-1200-80.jpg.webp", Price = 59.99m, Description = "One of the finest duos to ever grace the gaming world, there are few pairs you’ll spend a better time with than this particular Lombax and his robot pal. The plot at heart is simple enough - our unlikely twosome travel from planet to planet in an attempt to keep the galaxy safe from the egotists that threaten it. With plenty of laughs, puzzles to solve, and hoverboard races to take part in, Ratchet and Clank: Rift Apart's heroes serve as two of the most easily entertaining characters out there and joining them on their missions never once involves a dull moment." },
                    new Game() { Name = "Far Cry Primal", GenreId = 2, ImageUrl = "https://assets2.ignimgs.com/2015/10/06/far-cry-primal-button-1jpg-9966fe.jpg", Price = 59.99m, Description = "Far Cry Primal is a 2016 action-adventure game developed by Ubisoft Montreal and published by Ubisoft. It is the tenth installment in the Far Cry series. Set during prehistoric times, the game follows the story of Takkar, who starts off as an unarmed hunter but will rise to become the leader of a tribe, using his special gift of taming animals. Primal adapts the traditional Far Cry formula to its prehistoric setting, replacing modern firearms with primitive weapons like spears, clubs, and bows and arrows, and allowing players to summon animal companions during battles. Everyday survival is a key aspect of the gameplay, as players have to watch out for both natural predators and rival tribesmen." },
                    new Game() { Name = "God of War", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/iWfirewBN5qYa4QGGpBzWd-1200-80.jpg.webp", Price = 49.99m, Description = "Anyone that owned a console back in 2005 surely remembers the trials and tribulations of Kratos, the infamous Spartan god and harbinger of destruction in the ancient world, no? Even if you didn’t play the original God of War series, the reboot made for the PS4 in 2018 is brilliant. In a touching story centred around father and son, you assume the role of Kratos in what initially appears to be an unassuming journey to scatter the ashes of his late wife (and his son’s mother) and soon end up embroiled in the wrath of Odin, the whispers of the World Serpent and the stories of all sorts of other mythical figures." },
                    new Game() { Name = "Assassin's Creed Valhalla", GenreId = 2, ImageUrl = "https://cdn1.epicgames.com/salesEvent/salesEvent/EN_ACV_DLC_1_EPIC_Key_Art_Wide_3840x2160_3840x2160-40b1d9f5d6ec6b125e088d7959364bcd", Price = 39.99m, Description = "Wrath of the Druids is a downloadable content expansion package for Assassin's Creed: Valhalla that was released on 13 May 2021. It is the first out of three expansion packs for the game, alongside The Siege of Paris and Dawn of Ragnarök." },
                    new Game() { Name = "Solium Infernum", GenreId = 3, ImageUrl = "https://videogames.si.com/.image/c_limit%2Ccs_srgb%2Cq_auto:good%2Cw_700/MjAwNjI5NTY4NzM2ODYzMzQw/solium-infernum-1.webp", Price = 39.99m, Description = "Based on a classic, the remake basically answers the question of what Game of Thrones would look like if it took place in hell. You are one of the demon lords vying for hell’s throne, but all-out war is forbidden – as a true devil, you need to intrigue and backstab, find loopholes, and lie to gain power. You better play this one with very good friends. For gameplay impressions, check out our Solium Infernum first look from gamescom 2023." },
                    new Game() { Name = "Homeworld 3", GenreId = 3, ImageUrl = "https://videogames.si.com/.image/c_limit%2Ccs_srgb%2Cq_auto:good%2Cw_700/MTk4MjE0NjA3OTQ3NDQxNzEw/homeworld-3-1.webp", Price = 59.99m, Description = "After a series of delays fans will finally be able to dive into the iconic Homeworld series’ next chapter – and after a ground-based spin-off, we’re finally back in space! Featuring the intense space combat of its predecessors, Homeworld 3 adds an additional tactical layer by introducing terrain into the equation: From asteroid fields to the burnt-out hulls of ancient space hulks, you can expect your three-dimensional battles to be even more tactical. The game will also feature a quick-paced co-op roguelike mode for the first time aside from multiplayer and a campaign." },
                    new Game() { Name = "Manor Lords", GenreId = 3, ImageUrl = "https://videogames.si.com/.image/c_limit%2Ccs_srgb%2Cq_auto:good%2Cw_700/MjAxNzQxNzgyOTk4ODUzMjYz/manor-lords-battle-formation-1.webp", Price = 29.99m, Description = "Best described as a medieval city-builder that incorporates Total War-style battles, Manor Lords is one of the most highly anticipated strategy games in years. Manage your domain carefully as you contend not only with harsh winters and plagues, but also with envious neighboring lords who’d love nothing more than to take your flourishing lands for themselves. Beware of the cost of war, though, for every man lost in battle equals one missing worker back home." },
                    new Game() { Name = "City of Atlantis", GenreId = 3, ImageUrl = "https://videogames.si.com/.image/c_limit%2Ccs_srgb%2Cq_auto:good%2Cw_700/MjAyOTk3MjU3NTA1NjEyODEy/city-of-atlantis-1.webp", Price = 39.12m, Description = "Atlantis has reemerged from the depths and put you in charge to rebuild the legendary ocean city – hopefully able to withstand both enemies and nature this time around. Make sure your city has enough goods and space to satisfy its inhabitants, expand with impressive sections and bridges, and erect powerful defenses against anyone envious of your success." },
                    new Game() { Name = "Crash Team Racing: Nitro-Fueled", GenreId = 4, ImageUrl = "https://gmedia.playstation.com/is/image/SIEPDC/crash-team-racing-screen-07-ps4-en-30may19?$1600px$", Price = 39.99m, Description = "Naughty Dog’s exciting Crash Team Racing kart racer of 1999 is back with remastered visuals and additional vehicles and tracks. Mixing 1990s game feel with modern console presentation, Nitro-Fueled is the best way to revisit a high-speed classic." },
                    new Game() { Name = "Disney Speedstorm", GenreId = 4, ImageUrl = "https://gmedia.playstation.com/is/image/SIEPDC/disney-speedstorm-screenshot-01-en-22march24?$1600px$", Price = 9.99m, Description = "Classic characters from Disney and Pixar go head-to-head in this free-to-play combat racing extravaganza which offers regular updates." },
                    new Game() { Name = "Gran Turismo 7", GenreId = 4, ImageUrl = "https://gmedia.playstation.com/is/image/SIEPDC/gran-turismo-7-screenshot-01-ps5-en-09mar22?$1600px$", Price = 69.99m, Description = "The Real Driving Simulator has something for everyone, be they a committed and competitive racer or a casual driver looking for immediate thrills." },
                    new Game() { Name = "MotoGP 24", GenreId = 4, ImageUrl = "https://gmedia.playstation.com/is/image/SIEPDC/motogp-24-screenshot-02-en-22march24?$1600px$", Price = 54.99m, Description = "Challenge yourself across MotoGP, Moto2 and Moto3 categories in this new two-wheeled thrill ride, featuring official rosters and tracks from real-world competition." },
                    new Game() { Name = "Spider-Man 2", GenreId = 1, ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202306/1219/1c7b75d8ed9271516546560d219ad0b22ee0a263b4537bd8.png", Price = 79.99m, Description = " In this installment, Peter Parker and Miles Morales struggle to navigate the next steps in their personal lives while combating several new threats, including a private militia led by Kraven the Hunter, who transforms New York City into a hunting ground for super-powered individuals; and the extraterrestrial Venom symbiote, which bonds itself to Parker and negatively influences him, threatening to destroy his personal relationships." },
                    new Game() { Name = "Red Dead Redemption 2", GenreId = 1, ImageUrl = "https://cdn.akamai.steamstatic.com/steam/apps/1174180/header.jpg?t=1695140956", Price = 59.99m, Description = "America, 1899. The end of the Wild West era has begun. After a robbery goes badly wrong in the western town of Blackwater, Arthur Morgan and the Van der Linde gang are forced to flee. With federal agents and the best bounty hunters in the nation massing on their heels, the gang must rob, steal and fight their way across the rugged heartland of America in order to survive. As deepening internal divisions threaten to tear the gang apart, Arthur must make a choice between his own ideals and loyalty to the gang who raised him." },
                    new Game() { Name = "Call of Duty: Modern Warfare II", GenreId = 1, ImageUrl = "https://static.posters.cz/image/1300/posters/call-of-duty-modern-warfare-2-task-force-i149551.jpg", Price = 37.50m, Description = "Modern Warfare II's multiplayer component features several new game modes: Knockout, in which two teams attempt to capture a package with limited lives; and Prisoner Rescue, in which an attacking team attempts to extract a hostage while a defending team prevents them by fortifying defenses around the hostage." },
                    new Game() { Name = "Call of Duty: Modern Warfare III", GenreId = 1, ImageUrl = "https://www.callofduty.com/content/dam/atvi/callofduty/cod-touchui/store/games/mw3/overview/Store_GamesPDP_Hero01.png", Price = 45.49m, Description = "Modern Warfare III features similar gameplay to its predecessor and other games in the Call of Duty series. The game's single player campaign features a mixture of linear missions, and open-world missions known as \"Open Combat Missions\". In these missions, players have greater freedom to choose how they want to approach designated objectives, choosing between open combat to stealth and ambush tactics." },
                    new Game() { Name = "NBA 2K24", GenreId = 5, ImageUrl = "https://assets.xboxservices.com/assets/56/60/56600f4d-7994-4c7d-b240-8753529c1e4c.jpg?n=NBA-2K24_GLP-Page-Hero-0_1083x1222_02.jpg", Price = 78.90m, Description = "NBA 2K24 is a 2023 basketball video game developed by Visual Concepts Austin and published by 2K, based on the National Basketball Association (NBA). It is the 25th installment in the NBA 2K franchise and is the successor to NBA 2K23. The game was released on September 8, 2023, for Microsoft Windows, Nintendo Switch, PlayStation 4, PlayStation 5, Xbox One and Xbox Series X/S. The NBA 2K24 Arcade Edition was released on Apple Arcade on October 24, 2023." },
                    new Game() { Name = "EA Sports FC 24", GenreId = 5, ImageUrl = "https://s25.q4cdn.com/103200307/files/doc_multimedia/FC24_standardkeyart_16x9.jpg", Price = 75.60m, Description = "EA Sports FC 24 is an association football-themed simulation video game developed by EA Vancouver and EA Romania and published by EA Sports. It is the inaugural installment in the EA Sports FC series, succeeding the FIFA video game series after Electronic Arts's partnership with FIFA concluded with FIFA 23. EA Sports FC 24 is the 31st overall installment of EA Sports' football simulation games, and was released on 29 September 2023 for Nintendo Switch, PlayStation 4, PlayStation 5, Windows, Xbox One, and Xbox Series X/S." },
                    new Game() { Name = "WWE 2K22", GenreId = 5, ImageUrl = "https://shared.steamstatic.com/store_item_assets/steam/apps/1255630/header.jpg?t=1704736969", Price = 69.99m, Description = "WWE 2K's MyCareer was rebranded as MyRISE and focused on separate stories for both male and female wrestlers. Using created characters, players can form alliances, make enemies, and branch into subplots based upon the players choices. This game did see the removal of the Advance Entrance option when creating a superstars custom entrance, instead leaving the only option to Easy Entrance." },
                    new Game() { Name = "CS 1.6", GenreId = 1, ImageUrl = "https://tsarvar.com/img/game/cs1.6_icon_200px.jpg", Price = 35.00m, Description = "This is counter strike game" },
                    new Game() { Name = "MLB The Show 24", GenreId = 5, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/d/d8/MLB_The_Show_24_Cover.jpg", Price = 64.99m, Description = "MLB The Show 24 is a video game based on Major League Baseball developed by San Diego Studio and published by MLB Advanced Media and Sony Interactive Entertainment. It was released on March 19, 2024, for Nintendo Switch, PlayStation 4, PlayStation 5, Xbox One, and Xbox Series X/S. It is the nineteenth entry in the MLB: The Show game series and features Vladimir Guerrero Jr. on its cover" },
                    new Game() { Name = "Horizon Forbidden West", GenreId = 1, ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202107/3100/HO8vkO9pfXhwbHi5WHECQJdN.png", Price = 67.09m, Description = "Horizon Forbidden West is a 2022 action role-playing game developed by Guerrilla Games and published by Sony Interactive Entertainment. The sequel to Horizon Zero Dawn (2017), the game is set in a post-apocalyptic version of the Western United States, recovering from the aftermath of an extinction event caused by a rogue robot swarm. The player, assuming control of Aloy, must venture into the Forbidden West to find the source of a mysterious plague that kills all it infects. The player can explore the open world and complete quests using ranged and melee weapons against machine creatures and hostile rebels. Forbidden West introduced new gameplay mechanics to the franchise, such as new traversal tools, underwater exploration, and expanded melee combat." },
                    new Game() { Name = "Jurassic World Evolution 2", GenreId = 3, ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202107/2708/WZhQmSMCGEJgLHhnikOcuMnu.png", Price = 84.97m, Description = "Jurassic World Evolution 2 is the much-anticipated sequel to Frontier’s highly successful Jurassic World Evolution. This bold new era in the Jurassic World Evolution game franchise builds upon the ground-breaking and immersive 2018 management simulation." },
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}