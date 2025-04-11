using MyMovies.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class Movie
    {
        private string trailer;


        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("url")]
        public string Url { get; set; }


        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("year")]
        public int Year { get; set; }


        [JsonPropertyName("rating")]
        public double Rating { get; set; }


        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }


        [JsonPropertyName("summary")]
        public string Summary { get; set; }


        [JsonPropertyName("language")]
        public string Language { get; set; }


        [JsonPropertyName("background_image")]
        public string BackgroundImgUrl { get; set; }


        [JsonPropertyName("medium_cover_image")]
        public string CoverImgUrl { get; set; }


        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }


        [JsonPropertyName("yt_trailer_code")]
        public string Trailer
        {
            get { return trailer; }
            set { trailer = $"https://www.youtube.com/watch?v={value}"; }
        }

        public MovieState MovieState { get; set; } = MovieState.Favorite;
    }
}
