using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using RozmieniarkaApp.Services;
using System.Collections.ObjectModel;

namespace RozmieniarkaApp.ViewModels
{
    public partial class TaxWizardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isPageRefreshing;
        [ObservableProperty]
        private Color isTaxingEnabled;
        [ObservableProperty]
        private Color isCarWashWorking;
        private int taxingStatus;
        private int CarWashStatus;
        public TaxWizardPageViewModel()
        {
            taxingStatus = -1;
            CarWashStatus = -1;
            UpdateStatusLabelText();
            Task.Run(() => { RefreshTaxingStatus(); });

        }
        private void UpdateStatusLabelText()
        {
            List<int> statusList = new List<int>(2);
            statusList.Add(taxingStatus);
            statusList.Add(CarWashStatus);
            List<Color> colorList = new List<Color>(2);
            colorList.Add(IsTaxingEnabled);
            colorList.Add(IsCarWashWorking);
            for (int i = 0; i < statusList.Count(); i++)
                switch (statusList[i])
                {
                    case 0:
                        colorList[i] = Colors.Red;
                        break;
                    case 1:
                        colorList[i] = Colors.Green;
                        break;
                    default:
                        colorList[i] = Colors.White;
                        break;
                }
            IsTaxingEnabled = colorList[0];
            IsCarWashWorking = colorList[1];
        }
        [RelayCommand]
        public async Task RefreshTaxingStatus()
        {
            try
            {
                taxingStatus = await TaxWizardConnectionService.GetTaxingStatus();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd!", e.Message, "Ok");
            }
            finally
            {
                UpdateStatusLabelText();
                IsPageRefreshing = false;
            }
        }
        public async Task SetTaxingStatus(bool status)
        {
            try
            {
                taxingStatus = await TaxWizardConnectionService.SetTaxingStatus(status);
                UpdateStatusLabelText();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Błąd!", "Nie udało sie ustawić fiskalizacji!", "Ok");
            }
        }
        [RelayCommand]
        public async Task TurnOnButtonClicked()
        {
            await SetTaxingStatus(true);
        }
        [RelayCommand]
        public async Task TurnOffButtonClicked()
        {
            await SetTaxingStatus(false);
        }
        [RelayCommand]
        public async Task TopUpCarWashCredit(string amountStr)
        {

            bool answer = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz doładować?", "Doładuj", "Anuluj");
            if (!answer)
                return;
            try
            {
                int amount;
                Int32.TryParse(amountStr, out amount);
                await TaxWizardConnectionService.TopUpCarWashCredit(amount);
                await Toast.Make("Doładowano!", ToastDuration.Short, 14).Show();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd!", e.Message, "Ok");
            }
        }
    }
}
