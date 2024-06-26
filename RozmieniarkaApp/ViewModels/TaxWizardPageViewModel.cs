using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using RozmieniarkaApp.Services;
using System.Collections.ObjectModel;
using RozmieniarkaApp.Models;

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
        public TaxWizardPageViewModel()
        {
            IsTaxingEnabled = Colors.White;
            IsCarWashWorking = Colors.White;
            Task.Run(() => { RefreshPage(); });

        }
        private void InsertStatuses(TWStatusModel status)
        {
            IsTaxingEnabled = status.IsTaxingEnabled == 1 ? Colors.Green :
                status.IsTaxingEnabled == 0 ? Colors.Red :
                Colors.White;
            IsCarWashWorking = status.IsCarWashWorking == 1 ? Colors.Green :
                status.IsCarWashWorking == 0 ? Colors.Red :
                Colors.White;
        }
        [RelayCommand]
        public async Task RefreshPage()
        {
            TWStatusModel tWStatusModel = new();
            try
            {
                tWStatusModel = await TaxWizardConnectionService.GetStatuses();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd!", e.Message, "Ok");
            }
            finally
            {
                InsertStatuses(tWStatusModel);
                IsPageRefreshing = false;
            }
        }
        public async Task SetTaxingStatus(bool status)
        {
            try
            {
                await TaxWizardConnectionService.SetTaxingStatus(status);
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Błąd!", "Nie udało sie ustawić fiskalizacji!", "Ok");
            }
            finally
            {
                RefreshPage();
            }

        }
        [RelayCommand]
        public async Task TurnOnButtonClicked()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz włączyć?", "Doładuj", "Anuluj");
            if (!answer)
                return;
            await SetTaxingStatus(true);
        }
        [RelayCommand]
        public async Task TurnOffButtonClicked()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz wyłączyć?", "Doładuj", "Anuluj");
            if (!answer)
                return;
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
