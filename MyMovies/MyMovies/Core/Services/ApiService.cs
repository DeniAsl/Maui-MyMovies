using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMovies.Core.Services
{
    public class ApiService
    {
        private HttpClient _httpClient;
        private int _movieCount;

        public ApiService()
        {
            _httpClient = new();
        }

        public async Task<int> GetMovieCount()
        {
            return _movieCount;
        }

        public async Task<List<Movie>> GetMoviesByQueryAsync(string query)
        {
            string url = $"https://yts.mx/api/v2/list_movies.json?query_term={query}";
            return await GetApiResponseAsync(url);
        }

        public async Task<List<Movie>> GetMoviesAsync(int limit, int page, string genre)
        {
            if (genre == "None")
                genre = "";
            string url = $"https://yts.mx/api/v2/list_movies.json?limit={limit}&page={page}&genre={genre}";
            return await GetApiResponseAsync(url);
        }

        public async Task<List<Movie>> GetRelatedMoviesAsync(int id)
        {
            string url = $"https://yts.mx/api/v2/movie_suggestions.json?movie_id={id}";
            return await GetApiResponseAsync(url);
        }

        private async Task<List<Movie>> GetApiResponseAsync(string url)
        {
            string response = await _httpClient.GetStringAsync(url);
            ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(response);
            _movieCount = apiResponse.ApiData.MovieCount;
            return apiResponse.ApiData.Movies;
        }
    }
}
