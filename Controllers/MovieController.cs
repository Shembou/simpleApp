using Microsoft.AspNetCore.Mvc;
using simpleApp.Interfaces;
using simpleApp.Models;
using simpleApp.Utilities;
using System.Text.Json;

namespace simpleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Movie> movies = _movieService.GetMovies();
                return Ok(new { Message = "operation successfull", Data = movies });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult UpsertMovie([FromBody] JsonElement jsonElement)
        {
            var movie = JsonSerializer.Deserialize<Movie>(jsonElement.GetRawText());
            try
            {
                var response = _movieService.UpsertMovie(jsonElement);
                return Ok(new { Message = "operation successful", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult DeleteMovie([FromQuery] int id)
        {
            try
            {
                var deletedMovie = _movieService.DeleteMovie(id);
                return Ok(new { Message = "operation successful", Data = deletedMovie });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
