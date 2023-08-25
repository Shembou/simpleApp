using simpleApp.Models;
using System.Text.Json;

namespace simpleApp.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetMovies();
        Task<Movie> UpsertMovie(JsonElement jsonElement);

        Movie DeleteMovie(int id);
    }
}
