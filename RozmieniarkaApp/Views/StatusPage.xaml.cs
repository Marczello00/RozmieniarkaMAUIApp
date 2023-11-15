using RozmieniarkaApp.ViewModels;

namespace RozmieniarkaApp.Views;

public partial class StatusPage : ContentPage
{
    public StatusPage()
	{
		InitializeComponent();
        Task.Run(AnimateMask);
    }
        private async void AnimateMask()
    {
        Action<double> tealMovement = tInput => {
            firstStop.Offset = (float)tInput;
            firstStop2.Offset = (float)tInput;
            firstStop3.Offset = (float)tInput;
            firstStop4.Offset = (float)tInput;
            firstStop5.Offset = (float)tInput;
            firstStop6.Offset = (float)tInput;
            firstStop7.Offset = (float)tInput;
            firstStop8.Offset = (float)tInput;
        };
        Action<double> orangeMovement = oInput => {
            secondStop.Offset = (float)oInput;
            secondStop2.Offset = (float)oInput;
            secondStop3.Offset = (float)oInput;
            secondStop4.Offset = (float)oInput;
            secondStop5.Offset = (float)oInput;
            secondStop6.Offset = (float)oInput;
            secondStop7.Offset = (float)oInput;
            secondStop8.Offset = (float)oInput;
        };

        while (true)
        {
            StatusItemsGrid.Animate(name: "forward", callback: tealMovement, start: 0, end: 1, length: 1000, easing: Easing.SinIn);
            await Task.Delay(1000);
            StatusItemsGrid.Animate(name: "backward", callback: tealMovement, start: 1, end: 0, length: 1000, easing: Easing.SinIn);
            await Task.Delay(1000);

            StatusItemsGrid.Animate(name: "forward2", callback: orangeMovement, start: 1, end: 0, length: 1000, easing: Easing.SinIn);
            await Task.Delay(1000);
            StatusItemsGrid.Animate(name: "backward2", callback: orangeMovement, start: 0, end: 1, length: 1000, easing: Easing.SinIn);
            await Task.Delay(1000);
        }
    }

}