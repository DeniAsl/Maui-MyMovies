using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Models;
using MyMovies.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMovies.ViewModels
{
    [QueryProperty(nameof(ImdbCode), nameof(ImdbCode))]
    public class MovieSourcesViewModel : ObservableObject
    {
        private readonly SourceService _sourceService;

        public MovieSourcesViewModel(SourceService sourceService)
        {
            _sourceService = sourceService;
        }

        private ObservableCollection<Source> sources = new();
        public ObservableCollection<Source> Sources
        {
            get { return sources; }
            set
            {
                SetProperty(ref sources, value);
            }
        }

        private string imdbCode;
        public string ImdbCode
        {
            get { return imdbCode; }
            set
            {
                SetProperty(ref imdbCode, value);
            }
        }

        public ICommand GetSourcesCommand => new Command(async () =>
        {
             Sources = new ObservableCollection<Source>(await _sourceService.GetSourcesAsync(ImdbCode));
        });

        public ICommand OpenTrailerCommand => new Command<string>(async (magnet) =>
        {
            await Launcher.OpenAsync(magnet);
        });
    }
}
