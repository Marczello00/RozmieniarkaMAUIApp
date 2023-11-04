using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class WithdrawnPage : ContentPage
{
	public WithdrawnPage()
	{
		InitializeComponent();
		BindingContext = new WithdrawnPageViewModel();
	}
}