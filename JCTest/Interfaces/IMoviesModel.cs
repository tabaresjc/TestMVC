using JCTest.Models.Movies;
using System.Collections.Generic;

namespace JCTest.Models.Interfaces
{
    public interface IMoviesModel
    {
        MovieInfo Get(int movieId);
        IList<MovieInfo> GetList(int page = 1, int limit = 100, string orderby = "popularity_desc");
        void AddOrUpdate(IList<MovieInfo> movies);
    }
}