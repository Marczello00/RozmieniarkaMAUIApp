namespace RozmieniarkaApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
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
            bool answer = await DisplayAlert("Wyjście", "Czy na pewno chcesz wyjść?", "Tak", "Nie");
            if (answer)
            {
                Application.Current.Quit();
            }
        }
    }
}