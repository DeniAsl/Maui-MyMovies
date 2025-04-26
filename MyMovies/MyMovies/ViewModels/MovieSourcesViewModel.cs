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

        private bool isLoading = true;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                SetProperty(ref isLoading, value);
            }
        }

        private Source selectedSource;
        public Source SelectedSource
        {
            get { return selectedSource; }
            set
            {
                SetProperty(ref selectedSource, value);
                if (selectedSource != null)
                    OpenMagnetCommand.Execute(null);
            }
        }

        public ICommand GetSourcesCommand => new Command(async () =>
        {
            IsLoading = true;

             Sources = new ObservableCollection<Source>(await _sourceService.GetSourcesAsync(ImdbCode));

            IsLoading = false;
        });

        public ICommand OpenMagnetCommand => new Command(async () =>
        {
            await Launcher.OpenAsync(SelectedSource.InfoHash);

            SelectedSource = null;
        });
    }
}
