using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class SettingsPage : ContentPage
{
    SettingsViewModel _viewModel;
    public SettingsPage(SettingsViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        _viewModel.GetSettingsCommand?.Execute(null);
        base.OnAppearing();
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox)
        {
            _viewModel.ReceiveNotifications = checkBox.IsChecked;
        }
    }
}
