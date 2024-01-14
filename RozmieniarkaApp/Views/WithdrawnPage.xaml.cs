using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class WithdrawnPage : ContentPage
{
	public WithdrawnPage()
	{
		InitializeComponent();
		BindingContext = new WithdrawnPageViewModel();
        var viewModel = BindingContext as WithdrawnPageViewModel;
    }
    protected override bool OnBackButtonPressed()
    {
        MakeSureUserWantsToExit();
        return true;
    }
    private async void MakeSureUserWantsToExit()
    {
        bool answer = await DisplayAlert("Wyjœcie", "Czy na pewno chcesz wyjœæ?", "Tak", "Nie");
        if (answer)
        {
            Application.Current.Quit();
        }
    }
}