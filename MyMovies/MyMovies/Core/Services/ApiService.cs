using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMovies.Core.Services
{
    public class ApiService : IApiService
    {
        private HttpClient _httpClient;
        public ApiService()
        {
            _httpClient = new();
        }

        public async Task<List<Movie>> GetMoviesByGenreAsync(string genre)
        {
            string url = $"https://yts.mx/api/v2/list_movies.json?genre={genre}";
            return await GetApiResponseAsync(url);
        }

        public async Task<List<Movie>> GetMoviesByQueryAsync(string query)
        {
            string url = $"https://yts.mx/api/v2/list_movies.json?query_term={query}";
            return await GetApiResponseAsync(url);
        }

        public async Task<List<Movie>> GetRandomMoviesAsync(int limit, int page)
        {
            string url = $"https://yts.mx/api/v2/list_movies.json?limit={limit}&page={page}";
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
            return apiResponse.ApiData.Movies;
        }
    }
}
