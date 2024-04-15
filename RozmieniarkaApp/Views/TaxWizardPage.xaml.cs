using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class TaxWizardPage : ContentPage
{
	public TaxWizardPage()
	{
		InitializeComponent();
		BindingContext = new TaxWizardPageViewModel();
	}
}