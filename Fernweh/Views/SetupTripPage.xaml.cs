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

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    break;

                case DraggingCardPosition.UnderThreshold:
                    view.BackgroundColor = Color.DarkTurquoise;
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
                            view.BackgroundColor = Color.MediumPurple;
                            break;
                    }

                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    view.BackgroundColor = Color.Beige;
                    break;

                case DraggingCardPosition.FinishedOverThreshold:
                    view.BackgroundColor = Color.Beige;
                    directionLabel.Text = string.Empty;
                    positionLabel.Text = string.Empty;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}