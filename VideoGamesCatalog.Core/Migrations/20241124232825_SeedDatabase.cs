using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGamesCatalog.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			INSERT INTO GENRE([Name])
			SELECT 'Action'
			UNION
			SELECT 'Adventure'
			UNION
			SELECT 'Family'
			UNION
			SELECT 'Horror'
			UNION
			SELECT 'Racing'
			UNION
			SELECT 'Sports'

			GO

			INSERT INTO Platform([Name])
			SELECT 'PS4'
			UNION
			SELECT 'XBox'
			UNION
			SELECT 'Nintendo Switch'
			UNION
			SELECT 'PS5'

			GO

			INSERT INTO AgeRating([Description])
			SELECT 'Everyone'
			UNION
			SELECT '10+'
			UNION
			SELECT '13+'
			UNION
			SELECT '17+'

			GO


			SET IDENTITY_INSERT VideoGameImages ON;
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(1, '/images/Ape-On-The-Loose.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(2, '/images/Bee-Simulator.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(3, '/images/Legend-of-Zelda.jpg')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(4, '/images/Mario-and-Luigi.jpg')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(5, '/images/Chernobyl.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(6, '/images/After-Us.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(7, '/images/A-Hat-in-Time.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(8, '/images/Hot-Shot-Tennis.png')
			INSERT INTO VideoGameImages(ID, ImageUrl) VALUES(9, '/images/Animal-Well.png')
			SET IDENTITY_INSERT VideoGameImages OFF;

			GO

			DECLARE @EveryoneRatingID int = (SELECT ID FROM AgeRating WHERE Description ='Everyone')
			DECLARE @10PlusRatingID int = (SELECT ID FROM AgeRating WHERE Description = '10+')
			DECLARE @PS4 int = (SELECT ID FROM Platform WHERE Name = 'PS4')
			DECLARE @Nintendo int = (SELECT ID FROM Platform WHERE Name = 'Nintendo Switch')

			SET IDENTITY_INSERT VideoGames ON; 
			INSERT INTO VideoGames(ID, Title, Description, ReleaseDate, Price, AgeRatingID, PlatformID, ImageId)
			SELECT 100,
				   'Ape Escape: On the Loose', 
				   'A horde of crazy apes has stolen the Professor’s time machine and travelled back through history to alter the past and ensure that monkeykind rules the world. It’s up to Spike – and his collection of incredible gadgets – to save the day.',
				   '2023-08-25',
				   15.99,
				   @EveryoneRatingID,
				   @PS4,
				   1
			UNION
			SELECT 101,
				   'Bee Simulator',
				   'See the world through the eyes of a bee! Explore a world inspired by Central Park where you can take part in bee races, collect pollen from rare flowers and defy dangerous wasps.',
				   '2024-01-05',
				   10.49,
				   @EveryoneRatingID,
				   @PS4,
				   2
			UNION
			SELECT 102,
				   'The Legend of Zelda',
				   'Go on an adventure and take a breath of the wild with The Legend of Zelda: Breath of the Wild for the Nintendo Switch.',
				   '2017-03-09',
				   79.99,
				   @10PlusRatingID,
				   @PS4,
				   3
			UNION
			SELECT 103,
				   'Mario & Luigi™: Brothership',
				   'Set sail on an island-hopping adventure starring Mario and Luigi The brothers return for a brand-new adventure on the high seas! A mysterious power has fractured the world of Concordia into many scattered islands, and it’s up to Mario and Luigi to reconnect them.',
				   '2024-11-07',
				   79,
				   @10PlusRatingID,
				   @Nintendo,
				   4
			UNION
			SELECT 104,
				   'Chernobylite Complete Edition',
				   'Chernobylite is a Science Fiction Survival Horror RPG from developers Farm 51. Set in the hyper-realistic, 3D scanned wasteland of Chernobyl’s exclusion zone, you’ll take on the role of Igor, a physicist and ex-employee of the Chernobyl Power Plant, returning to Pripyat to investigate the mysterious disappearance of his fiancee, 30 years prior.',
				   '2022-04-21',
				   39.99,
				   @10PlusRatingID,
				   @PS4,
				   5
			UNION
			SELECT 105,
				   'After Us',
				   'After Us is a touching platforming adventure about human impact, sacrifice and hope where you''ll need to navigate an abstract world and salvage the souls of extinct animals.',
				   '2022-08-12',
				   55.99,
				   @10PlusRatingID,
				   @PS4,
				   6
			UNION
			SELECT 106,
				   'A Hat in Time',
				   'A Hat in Time is a cute-as-heck 3D platformer featuring a little girl who stitches hats for wicked powers! Freely explore giant worlds and recover Time Pieces to travel to new heights!',
				   '2024-08-17',
				   10.99,
				   @EveryoneRatingID,
				   @Nintendo,
				   7
			UNION
			SELECT 107,
				   'Hot Shot Tennis',
				   'Finally, a Tennis game for everyone! Filled with classic Hot Shots personality and true tennis gameplay, play against a quirky case of tennis pros challenge friends and family in singles or even 4 player doubles matches.',
				   '2021-07-03',
				   15.99,
				   @EveryoneRatingID,
				   @PS4,
				   8
			UNION
			SELECT 108,
				   'Animal Well',
				   'Discover a mysterious, non-linear, interconnected world as you solve puzzles in over 250 rooms in whatever order you choose. Rely on your wits to avoid danger in this combat-free Metroidvania.',
				   '2020-04-19',
				   40.99,
				   @EveryoneRatingID,
				   @PS4,
				   9
			GO

			DECLARE @ActionGenre int = (SELECT ID FROM Genre WHERE Name = 'Action')
			DECLARE @AdventureGenre int = (SELECT ID FROM Genre WHERE Name = 'Adventure')
			DECLARE @FamilyGenre int = (SELECT ID FROM Genre WHERE Name = 'Family')
			DECLARE @Horror int = (SELECT ID FROM Genre WHERE Name = 'Horror')
			DECLARE @Sports int = (SELECT ID FROM Genre WHERE Name = 'Sports')

			INSERT INTO GenreVideoGame(GenresID, VideoGamesID)
			SELECT @ActionGenre, 100
			UNION
			SELECT @AdventureGenre, 100
			UNION
			SELECT @FamilyGenre, 100
			UNION
			SELECT @ActionGenre, 101
			UNION
			SELECT @FamilyGenre, 101
			SELECT @ActionGenre, 102
			UNION
			SELECT @AdventureGenre, 102
			UNION
			SELECT @ActionGenre, 103
			UNION
			SELECT @AdventureGenre, 103
			UNION
			SELECT @FamilyGenre, 103
			UNION
			SELECT @Horror, 104
			UNION
			SELECT @AdventureGenre, 105
			UNION
			SELECT @ActionGenre, 105
			UNION
			SELECT @FamilyGenre, 106
			UNION
			SELECT @FamilyGenre, 107
			UNION
			SELECT @Sports, 107
			UNION
			SELECT @Horror, 108
			UNION
			SELECT @ActionGenre, 108

			GO
			");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM GenreVideoGame");
            migrationBuilder.Sql("DELETE FROM VideoGames");
            migrationBuilder.Sql("DELETE FROM VideoGameImages");
            migrationBuilder.Sql("DELETE FROM Platform");
            migrationBuilder.Sql("DELETE FROM Genre");
            migrationBuilder.Sql("DELETE FROM AgeRating");
        }
    }
}
