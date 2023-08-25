using simpleApp.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace simpleApp.Utilities
{
    public class MovieUtilities
    {
        public static async Task<Movie> GetMovieDetailsFromMetacritic(string title)
        {
            var html = await GetMetacriticHtml(title);
            return GetDataFromHtml(html);
            
        }
        public static async Task<string> GetMetacriticHtml(string title)
        {
            title = title.Replace(" ", "-").ToLower();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                return await client.GetStringAsync($"https://www.metacritic.com/movie/{title}");
            }
        }
        public static Movie GetDataFromHtml(string html)
        {
            string genrePattern = @"MetaC\['genre'\]\s*=\s*""([^""]+)"";";
            string scorePattern = @"MetaC\['score'\]\s*=\s*""([^""]+)"";";
            var movie = new Movie
            {
                Title = GetTitle(html),
                Director = GetDirector(html),
                Genres = ExtractValue(html, genrePattern),
                Runtime = GetRuntime(html),
                Metascore = int.Parse(ExtractValue(html, scorePattern)),
            };
            return movie;

        }
        public static int GetRuntime(string html)
        {
            int runtimeStart = html.IndexOf("<span class=\"label\">Runtime:</span>") + "<span class=\"label\">Runtime:</span>".Length;
            int runtimeEnd = html.IndexOf("</span>", runtimeStart);

            if (runtimeStart >= 0 && runtimeEnd >= 0)
            {
                string runtime = html.Substring(runtimeStart, runtimeEnd - runtimeStart).Trim();
                runtime = Regex.Replace(runtime, "[^0-9 -]", "").Trim();
                return int.Parse(runtime);
            }
            else
            {
                return 0;
            }
        }
        public static string GetDirector(string html)
        {
            int directorStart = html.IndexOf("<span class=\"label\">Director:</span>") + "<span class=\"label\">Director:</span>".Length;
            int directorEnd = html.IndexOf("</a>", directorStart);

            if (directorStart >= 0 && directorEnd >= 0)
            {
                string directorSpan = html.Substring(directorStart, directorEnd - directorStart);
                directorSpan = directorSpan.Replace("\n", "").Replace("\r", "").Trim();
                int directorNameStart = directorSpan.IndexOf("<span>") + "<span>".Length;
                int directorNameEnd = directorSpan.IndexOf("</span>", directorNameStart);

                if (directorNameStart >= 0 && directorNameEnd >= 0)
                {
                    string directorName = directorSpan.Substring(directorNameStart, directorNameEnd - directorNameStart).Trim();
                    directorName = directorName.Replace("<span>", "").Trim();
                    return directorName;
                }
                else
                {
                    return "";
                }
            } 
            else {
                return "";
            }
        }
        public static string GetTitle(string html)
        {
            int titleStart = html.IndexOf("<title>") + "<title>".Length;
            int titleEnd = html.IndexOf("</title>", titleStart);

            if (titleStart >= 0 && titleEnd >= 0)
            {
                string title = html.Substring(titleStart, titleEnd - titleStart).Trim();
                title = title.Replace("Reviews - Metacritic", "").Trim();
                return title;
            }
            else
            {
                return "";
            }
        }
        public static string ExtractValue(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern);
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
        //TODO: Extract to global utilities
        public static T deserializeMovie<T>(JsonElement jsonElement)
        {
            return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
        }
    }
}
