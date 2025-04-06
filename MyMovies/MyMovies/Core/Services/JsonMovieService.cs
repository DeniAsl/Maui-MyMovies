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
    public class JsonMovieService : IJsonMovieService
    {
        private readonly string targetFile = $"{FileSystem.AppDataDirectory}, MyMovies.json";

        public async Task<bool> IsFavorite(int id)
        {
            Movie existingMovie = await GetById(id);
            if (existingMovie == null)
                return false;
            return true;
        }

        private void EmptyFile()
        {
            if (File.Exists(targetFile))
            {
                File.WriteAllText(targetFile, JsonSerializer.Serialize(new List<Movie>()));
            }
        }

        public int TotalNumberOfFavMovies()
        {
            EnsureFileExists();
            string savedSerialized = File.ReadAllText(targetFile);
            return JsonSerializer.Deserialize<List<Movie>>(savedSerialized).Count();
        }

        public async Task<List<Movie>> GetAll(int limit = int.MaxValue, int page = 1)
        {
            EnsureFileExists();
            string savedSerialized = await File.ReadAllTextAsync(targetFile);
            List<Movie> savedMovies = JsonSerializer.Deserialize<List<Movie>>(savedSerialized);
            return savedMovies.Skip(limit * (page - 1)).Take(limit).ToList();
        }
        public async Task<Movie> GetById(int id)
        {
            List<Movie> movies = await GetAll();
            Movie existingMovie = movies.FirstOrDefault(search =>
            {
                return search.Id == id;
            });
            return existingMovie;
        }
        public async Task<bool> Add(Movie movie)
        {
            List<Movie> movies = (await GetAll()).ToList();
            if (movies.Count > 0)
            {
                if (movies.Any(existingMovie => existingMovie?.Id == movie?.Id))
                    return false;
                else
                    movies.Add(movie);
            }
            else
                movies.Add(movie);

            await WriteMovies(movies);
            return true;
        }

        public async Task<bool> Remove(Movie movie)
        {
            List<Movie> movies = await GetAll();
            Movie existingMovie = movies.FirstOrDefault(search =>
            {
                return search.Id == movie.Id;
            });

            if (existingMovie != null)
                movies.Remove(existingMovie);
            else
                return false;

            await WriteMovies(movies);
            return true;
        }

        public async Task<bool> Update(Movie movie)
        {
            List<Movie> movies = await GetAll();
            Movie existingMovie = movies.FirstOrDefault(search =>
            {
                return search.Id == movie.Id;
            });

            if (existingMovie != null)
            {
                movies.Remove(existingMovie);
                movies.Add(movie);
            }
            else
                return false;

            await WriteMovies(movies);
            return true;
        }
        private void EnsureFileExists()
        {
            if (!File.Exists(targetFile))
            {
                File.WriteAllText(targetFile, JsonSerializer.Serialize(new List<Movie>()));
            }
        }
        private async Task WriteMovies(List<Movie> movies)
        {
            string serializedMovies = JsonSerializer.Serialize(movies);
            await File.WriteAllTextAsync(targetFile, serializedMovies);
        }

        //public async Task<Movie> ChooseRandom()
        //{
        //    var students = (await GetAll())
        //    .Where(student => student.IsPresent)
        //    .ToList();
        //    if (students.Count == 0) return null;
        //    Random random = new Random();
        //    int randomIndex = random.Next(0, students.Count);
        //    Student chosenStudent = students[randomIndex];
        //    chosenStudent.TimesChosen++;
        //    await Update(chosenStudent);
        //    return chosenStudent;
        //}
    }
}
