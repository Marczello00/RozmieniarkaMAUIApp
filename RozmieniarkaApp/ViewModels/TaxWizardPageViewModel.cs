using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RozmieniarkaApp.Services;
using System.Collections.ObjectModel;

namespace RozmieniarkaApp.ViewModels
{
    public partial class TaxWizardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isPageRefreshing;
        [ObservableProperty]
        private int taxingStatus;
        [ObservableProperty]
        private string statusLabelText;
        private string JakisText= "Lol";
        public ObservableCollection<string> TaxingStatusList { get; set; }
        public ObservableCollection<char> TaxingStatusList2 { get; set; }
        public TaxWizardPageViewModel()
        {
            TaxingStatusList = new ObservableCollection<string>
            {
                "Status: fiskalizacja włączona",
                "Status: fiskalizacja wyłączona",
                "Status: nieznany",
                "Status: błąd",
                JakisText
            };
            TaxingStatusList2 = new ObservableCollection<char>
            {
                'a',
                'b',
                'c',
                'd',
                'e'
            };

            TaxingStatus = -1;
            UpdateStatusLabelText();

        }
        public async Task SetTaxingStatus(bool status)
        {
            TaxingStatus = await TaxWizardConnectionService.SetTaxingStatus(status);
            UpdateStatusLabelText();
        }
        [RelayCommand]
        public async Task  TurnOnButtonClicked()
        {
            await SetTaxingStatus(true);
        }
        [RelayCommand]
        public async Task TurnOffButtonClicked()
        {
            await SetTaxingStatus(false);
        }
        [RelayCommand]
        public async Task RefreshTaxingStatus()
        {
            try
            {
                TaxingStatus = await TaxWizardConnectionService.GetTaxingStatus();
            }
            catch (Exception)
            {
            }
            finally
            {
                UpdateStatusLabelText();
                IsPageRefreshing = false;
            }
        }
        private void UpdateStatusLabelText()
        {
            if (TaxingStatus == 1)
            {
                StatusLabelText = "Status: fiskalizacja włączona";
            }
            else if (TaxingStatus == 0)
            {
                StatusLabelText = "Status: fiskalizacja wyłączona";
            }
            else if (TaxingStatus == -1)
            {
                StatusLabelText = "Status: nieznany";
            }
            else
            {
                StatusLabelText = "Status: błąd";
            }
            JakisText = StatusLabelText;
            TaxingStatusList.Add(new string(JakisText.ToCharArray()));
            TaxingStatusList2.Add('s');
        }
    }
}
