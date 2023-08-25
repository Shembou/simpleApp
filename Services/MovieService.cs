using Microsoft.EntityFrameworkCore;
using simpleApp.DAL;
using simpleApp.Interfaces;
using simpleApp.Models;
using simpleApp.Utilities;
using System.Text.Json;

namespace simpleApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationContext _dbContext;
        public MovieService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Movie> GetMovies()
        {
            return _dbContext.Movies.ToList();
        }
        public async Task<Movie> UpsertMovie(JsonElement jsonElement)
        {
            var movie = MovieUtilities.deserializeMovie<Movie>(jsonElement);
            movie = await MovieUtilities.GetMovieDetailsFromMetacritic(movie.Title);

            bool doesExist = _dbContext.Movies.Any(x => x.Title == movie.Title && x.Runtime == movie.Runtime); //just in case if there are same titles

            if (doesExist)
            {
                var selectedMovie = _dbContext.Movies.First(x => x.Title == movie.Title);
                movie.Id = selectedMovie.Id;
                selectedMovie = movie;
            }
            else
            {
                _dbContext.Movies.Add(movie);
            }
            _dbContext.SaveChanges();
            return movie;
        }
        public Movie DeleteMovie(int id)
        {
            Movie movieToDelete = _dbContext.Movies.First(x => x.Id == id);
            _dbContext.Remove(movieToDelete);
            _dbContext.SaveChanges();
            return movieToDelete;
        }
    }
}
