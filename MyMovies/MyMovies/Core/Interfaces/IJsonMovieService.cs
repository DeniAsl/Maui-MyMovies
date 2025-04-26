using MyMovies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Interfaces
{
    interface IJsonMovieService
    {
        Task<bool> IsFavorite(int id);
        int GetNumberOfFavMovies();
        Task<List<Movie>> GetAll(int limit, int page, string genre);
        Task<Movie> GetById(int id);
        Task<bool> Add(Movie movie);
        Task<bool> Remove(Movie movie);
        Task<bool> Update(Movie movie);
    }
}
