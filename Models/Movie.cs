using simpleApp.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace simpleApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Director { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Genres { get; set; }
        public int Runtime { get; set; }
        public int Metascore { get; set; }
    }
}
