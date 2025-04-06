using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Interfaces
{
    interface IApiService
    {
        Task<List<Movie>> GetRandomMoviesAsync(int limit, int page);
        Task<List<Movie>> GetMoviesByQueryAsync(string query);
        Task<List<Movie>> GetMoviesByGenreAsync(string genre);
        Task<List<Movie>> GetRelatedMoviesAsync(int id);
    }
}
