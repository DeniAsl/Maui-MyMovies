using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMovies.Core.Services
{
    public class SourceService
    {
        private HttpClient _httpClient;
        private const string _baseUrl = "https://torrentio.strem.fun/sort=size%7Cqualityfilter=other,scr,cam,unknown/stream/movie/";

        public SourceService()
        {
            _httpClient = new();
        }

        public async Task<List<Source>> GetSourcesAsync(string imdbCode)
        {
            string url = $"{_baseUrl}{imdbCode}.json";
            string response = await _httpClient.GetStringAsync(url.ToLower());

            var root = JsonSerializer.Deserialize<RootObject>(response);
            return root?.Streams ?? new List<Source>();
        }

        public class RootObject
        {
            [JsonPropertyName("streams")]
            public List<Source> Streams { get; set; }
        }

    }
}
