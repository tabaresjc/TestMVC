namespace JCTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.app_movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        Language = c.String(maxLength: 10, storeType: "nvarchar"),
                        Title = c.String(maxLength: 256, storeType: "nvarchar"),
                        OriginalTitle = c.String(maxLength: 256, storeType: "nvarchar"),
                        TagLine = c.String(maxLength: 1024, storeType: "nvarchar"),
                        Overview = c.String(maxLength: 2048, storeType: "nvarchar"),
                        Poster = c.String(maxLength: 2048, storeType: "nvarchar"),
                        Backdrop = c.String(maxLength: 2048, storeType: "nvarchar"),
                        Adult = c.Boolean(nullable: false),
                        Budget = c.Int(nullable: false),
                        HomePage = c.String(maxLength: 512, storeType: "nvarchar"),
                        Imdb = c.String(maxLength: 512, storeType: "nvarchar"),
                        ReleaseDate = c.DateTime(precision: 0),
                        Revenue = c.Long(nullable: false),
                        Runtime = c.Int(),
                        Popularity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VoteAverage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VoteCount = c.Int(nullable: false),
                        Status = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.MovieId, unique: true, clustered: true, name: "IX_MOVIE_ID")
                .Index(t => t.Language);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.app_movies", new[] { "Language" });
            DropIndex("dbo.app_movies", "IX_MOVIE_ID");
            DropTable("dbo.app_movies");
        }
    }
}
