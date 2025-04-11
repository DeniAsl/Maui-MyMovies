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
        private const string _baseUrl = "https://yts.mx/api/v2/list_movies.json?";

        public ApiService()
        {
            _httpClient = new();
        }

        public async Task<ApiResponse> GetMoviesByQueryAsync(string query, int limit, int page)
        {
            string url = $"{_baseUrl}query_term={query}&limit={limit}&page={page}";
            return await GetApiResponseAsync(url);
        }

        public async Task<ApiResponse> GetMoviesAsync(int limit, int page, string genre)
        {
            if (genre == "None")
                genre = "";
            string url = $"{_baseUrl}limit={limit}&page={page}&genre={genre}";
            return await GetApiResponseAsync(url);
        }
        public async Task<ApiResponse> GetRandomMoviesAsync(int limit, string genre, string sortBy = "rating", string orderBy = "desc")
        {
            if (genre == "None")
                genre = "";
            string url = $"{_baseUrl}limit={limit}&genre={genre}&sort_by={sortBy}&order_by={orderBy}";
            ApiResponse apiResponse = await GetApiResponseAsync(url);
            int moviesCount = apiResponse.ApiData.MovieCount;
            Random random = new Random();
            int randomPage = random.Next(1, (int)Math.Ceiling((double)moviesCount / limit) + 1);
            url += $"&page={randomPage}";
            return await GetApiResponseAsync(url);
        }

        public async Task<ApiResponse> GetRelatedMoviesAsync(int id)
        {
            string url = $"https://yts.mx/api/v2/movie_suggestions.json?movie_id={id}";
            return await GetApiResponseAsync(url);
        }

        private async Task<ApiResponse> GetApiResponseAsync(string url)
        {
            string response = await _httpClient.GetStringAsync(url.ToLower());
            return JsonSerializer.Deserialize<ApiResponse>(response);
        }
    }
}
