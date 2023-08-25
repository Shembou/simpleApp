using simpleApp.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace simpleApp.DAL
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Movies.Any())
            {
                if (context.Movies.Count() != 0)
                {
                    return;
                }
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");

            var Movies = new List<Movie>
            {
                new Movie{ Title = "Blue Beetle", Director = "Angel Manuel Soto", Genres = "Action, Adventure, Sci-Fi, Thriller", Runtime = 127, Metascore = 46},
                new Movie{ Title = "Gran Turismo", Director = "Neill Blomkamp", Genres = "Action, Adventure, Drama, Sport", Runtime = 135, Metascore = 61}
            };
            Movies.ForEach(movie => context.Movies.Add(movie));
            context.SaveChanges();

            var Users = new List<User>
            {
                new User{ Username = "admin", Password = hashedPassword, Email = "admin@gmail.com" }
            };
            Users.ForEach(user => context.Users.Add(user));
            context.SaveChanges();
        }
    }
}
