using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class InsertedPage : ContentPage
{
	public InsertedPage()
	{
		InitializeComponent();
		BindingContext = new InsertedPageViewModel();
    }
    protected override bool OnBackButtonPressed()
    {
        MakeSureUserWantsToExit();
        return true;
    }
    private async void MakeSureUserWantsToExit()
    {
        bool answer = await DisplayAlert("Wyj�cie", "Czy na pewno chcesz wyj��?", "Tak", "Nie");
        if (answer)
        {
            Application.Current.Quit();
        }
    }
}