using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class InsertedPage : ContentPage
{
	public InsertedPage()
	{
		InitializeComponent();
		BindingContext = new InsertedPageViewModel();
	}
}