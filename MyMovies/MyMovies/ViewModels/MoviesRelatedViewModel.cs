using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Interfaces;
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
        private IJsonMovieService _jsonMovieService;

        public MoviesRelatedViewModel(IJsonMovieService jsonMovieService)
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

        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                SetProperty(ref selectedMovie, value);
                if (selectedMovie != null)
                    ShowMovieCommand.Execute(null);
            }
        }

        public ICommand ShowMovieCommand => new Command(async () =>
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(MovieDetailsViewModel.SelectedMovie), SelectedMovie },
                { nameof(MovieDetailsViewModel.IsFavorite), await _jsonMovieService.IsFavorite(SelectedMovie.Id) }
            };

            SelectedMovie = null;

            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}", navigationParameter);
        });
    }
}
