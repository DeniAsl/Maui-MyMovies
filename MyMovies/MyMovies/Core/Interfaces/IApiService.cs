using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Interfaces
{
    public interface IApiService
    {
        Task<ApiResponse> GetMoviesAsync(int limit, int page, string genre, string sortBy, string orderBy);
        Task<ApiResponse> GetMoviesByQueryAsync(string query, int limit, int page);
        Task<ApiResponse> GetRelatedMoviesAsync(int id);
        Task<ApiResponse> GetRandomMoviesAsync(int limit, string genre, string sortBy, string orderBy);
    }
}
