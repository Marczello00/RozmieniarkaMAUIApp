using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class StatusPage : ContentPage
{
    public StatusPage()
	{
		InitializeComponent();
        BindingContext = new StatusPageViewModel();
    }

}