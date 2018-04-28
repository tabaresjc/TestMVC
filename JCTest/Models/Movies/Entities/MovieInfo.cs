using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JCTest.Models.Movies
{
    [Table("app_movies")]
    public class MovieInfo
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_MOVIE_ID", IsClustered = true, IsUnique = true)]
        public int MovieId { get; set; }

        [Index]
        [StringLength(10)]
        public string Language { get; set; }

        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(256)]
        public string OriginalTitle { get; set; }

        [StringLength(1024)]
        public string TagLine { get; set; }

        [StringLength(2048)]
        public string Overview { get; set; }

        [StringLength(2048)]
        public string Poster { get; set; }

        [StringLength(2048)]
        public string Backdrop { get; set; }
        
        public bool Adult { get; set; }

        public int Budget { get; set; }

        [StringLength(512)]
        public string HomePage { get; set; }

        [StringLength(512)]
        public string Imdb { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public Int64 Revenue { get; set; }

        public int? Runtime { get; set; }

        public decimal Popularity { get; set; }

        public decimal VoteAverage { get; set; }

        public int VoteCount { get; set; }

        public string Status { get; set; }

        private const string imageBaseUrl = "//image.tmdb.org/t/p/";

        private const string noImageUrl = "/Assets/images/no-image.png";

        private static Dictionary<string, string> posterSizeDict = new Dictionary<string, string>
        {
            { "xx-small", "w92" }, 
            { "x-small", "w154" },
            { "small", "w185" },
            { "medium", "w342" },
            { "large", "w500" },
            { "x-large", "w154" },
            { "xx-large", "w780" },
            { "original", "original" }
        };

        private static Dictionary<string, string> backdropSizeDict = new Dictionary<string, string>
        {
            { "small", "w300" },
            { "medium", "w780" },
            { "large", "w1280" },
            { "original", "original" }
        };

        public string GetPosterUrl(string size = "medium")
        {
            if (string.IsNullOrWhiteSpace(this.Poster))
            {
                return noImageUrl;
            }
            
            if (!posterSizeDict.ContainsKey(size))
            {
                size = "medium";
            }

            return string.Concat(
                imageBaseUrl,
                posterSizeDict[size],
                this.Poster
            );
        }

        public string GetBackdropUrl(string size = "medium")
        {
            if (string.IsNullOrWhiteSpace(this.Backdrop))
            {
                return noImageUrl;
            }

            if (!backdropSizeDict.ContainsKey(size))
            {
                size = "medium";
            }

            return string.Concat(
                imageBaseUrl,
                backdropSizeDict[size],
                this.Backdrop
            );
        }

        public static MovieInfo ConvertFrom(System.Net.TMDb.Movie m, string language = null)
        {
            return new MovieInfo
            {
                MovieId = m.Id,
                Language = language,
                Title = m.Title,
                OriginalTitle = m.OriginalTitle,
                TagLine = m.TagLine,
                Overview = m.Overview,
                Poster = m.Poster,
                Backdrop = m.Backdrop,
                Adult = m.Adult,
                Budget = m.Budget,
                HomePage = m.HomePage,
                Imdb = m.Imdb,
                ReleaseDate = m.ReleaseDate,
                Revenue = m.Revenue,
                Runtime = m.Runtime,
                Popularity = m.Popularity,
                VoteAverage = m.VoteAverage,
                VoteCount = m.VoteCount,
                Status = m.Status,
            };
        }
    }
}