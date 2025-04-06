using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class ApiData
    {
        [JsonPropertyName("movie_count")]
        public int MovieCount { get; set; }


        [JsonPropertyName("movies")]
        public List<Movie> Movies { get; set; }
    }
}
