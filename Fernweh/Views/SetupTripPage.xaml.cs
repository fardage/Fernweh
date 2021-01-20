using System;
using Fernweh.ViewModels;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class SetupTripPage : ContentPage
    {
        private readonly SetupTripViewModel viewModel;

        public SetupTripPage(SetupTripViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            SwipeCardView.Dragging += OnDragging;
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (View) sender;

            var directionLabel = view.FindByName<Label>("DirectionLabel");
            directionLabel.Text = e.Direction.ToString();

            var positionLabel = view.FindByName<Label>("PositionLabel");
            positionLabel.Text = e.Position.ToString();

            var lightColor = Color.FromHex(Application.Current.Resources["LightPrimaryPageBackgroundColor"].ToString());
            var darkColor = Color.FromHex(Application.Current.Resources["DarkSecondaryPageBackgroundColor"].ToString());

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    break;

                case DraggingCardPosition.UnderThreshold:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                    break;

                case DraggingCardPosition.OverThreshold:
                    switch (e.Direction)
                    {
                        case SwipeCardDirection.Left:
                            view.BackgroundColor = Color.FromHex("#FF6A4F");
                            break;

                        case SwipeCardDirection.Right:
                            view.BackgroundColor = Color.FromHex("#63DD99");
                            break;

                        case SwipeCardDirection.Up:
                            view.BackgroundColor = Color.FromHex("#2196F3");
                            break;

                        case SwipeCardDirection.Down:
                            break;
                    }

                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    view.BackgroundColor = Color.Beige;
                    break;

                case DraggingCardPosition.FinishedOverThreshold:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                    directionLabel.Text = string.Empty;
                    positionLabel.Text = string.Empty;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private async void DismissCard_Clicked(object sender, EventArgs e)
        {
            await SwipeCardView.InvokeSwipe(SwipeCardDirection.Left, 20, 10, new TimeSpan(1), new TimeSpan(200));
        }

        private async void AcceptCard_Clicked(object sender, EventArgs e)
        {
            await SwipeCardView.InvokeSwipe(SwipeCardDirection.Right, 20, 10, new TimeSpan(1), new TimeSpan(200));
        }
    }
}