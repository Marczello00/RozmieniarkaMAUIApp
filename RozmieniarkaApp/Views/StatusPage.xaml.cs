namespace RozmieniarkaApp.Views;

public partial class StatusPage : ContentPage
{
    public StatusPage()
	{
		InitializeComponent();
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