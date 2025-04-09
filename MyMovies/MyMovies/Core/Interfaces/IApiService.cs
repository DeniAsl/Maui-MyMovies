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
        Task<(List<Movie>, int)> GetMoviesAsync(int limit, int page, string genre);
        Task<List<Movie>> GetMoviesByQueryAsync(string query);
        Task<List<Movie>> GetRelatedMoviesAsync(int id);
    }
}
