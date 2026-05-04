namespace Policlinica.ViewModels;

public class Navigation
{
    private StartViewModel startViewModel;

    public void Navigate(ViewModelBase viewModel)
    {
        startViewModel.CurrentPage = viewModel;
    }

    public void SetCurrentView(StartViewModel startViewModel)
    {
        this.startViewModel = startViewModel;
    }

    public void Close()
    {
        this.startViewModel.Close();
    }
}