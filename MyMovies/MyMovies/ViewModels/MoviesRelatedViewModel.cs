using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Models;
using MyMovies.Core.Services;
using MyMovies.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMovies.ViewModels
{
    [QueryProperty(nameof(Movies), nameof(Movies))]
    [QueryProperty(nameof(SourceMovieTitle), nameof(SourceMovieTitle))]
    public class MoviesRelatedViewModel : ObservableObject
    {
        private JsonMovieService _jsonMovieService;

        public MoviesRelatedViewModel(JsonMovieService jsonMovieService)
        {
            _jsonMovieService = jsonMovieService;
        }

        private ObservableCollection<Movie> movies = new();
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set
            {
                SetProperty(ref movies, value);
            }
        }

        private string sourceMovieTitle;
        public string SourceMovieTitle
        {
            get { return sourceMovieTitle; }
            set
            {
                SetProperty(ref sourceMovieTitle, value);
            }
        }

        public ICommand ShowMovieCommand => new Command<Movie>(async (movie) =>
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(MovieDetailsViewModel.SelectedMovie), movie },
                { nameof(MovieDetailsViewModel.IsFavorite), await _jsonMovieService.IsFavorite(movie.Id) }
            };

            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}", navigationParameter);
        });
    }
}
