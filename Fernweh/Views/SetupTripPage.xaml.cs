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

        private void Done_Clicked(object sender, EventArgs e)
        {
            viewModel.WrapUpTrip();
        }

        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (View) sender;

            var nopeFrame = view.FindByName<Frame>("NopeFrame");
            var likeFrame = view.FindByName<Frame>("AddFrame");

            var threshold = (BindingContext as SetupTripViewModel).Threshold;
            var draggedXPercent = e.DistanceDraggedX / threshold;

            var lightColor = (Color) Application.Current.Resources["LightSecondaryPageBackgroundColor"];
            var darkColor = (Color) Application.Current.Resources["DarkSecondaryPageBackgroundColor"];

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);

                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    break;

                case DraggingCardPosition.UnderThreshold:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = -1 * draggedXPercent;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = draggedXPercent;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                    }

                    break;

                case DraggingCardPosition.OverThreshold:
                    switch (e.Direction)
                    {
                        case SwipeCardDirection.Left:
                            view.BackgroundColor = Color.FromHex("#FF6A4F");
                            nopeFrame.Opacity = 1;
                            break;

                        case SwipeCardDirection.Right:
                            view.BackgroundColor = Color.FromHex("#63DD99");
                            likeFrame.Opacity = 1;
                            break;

                        case SwipeCardDirection.Up:
                            view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                            nopeFrame.Opacity = 0;
                            likeFrame.Opacity = 0;
                            break;

                        case SwipeCardDirection.Down:
                            view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                            nopeFrame.Opacity = 0;
                            likeFrame.Opacity = 0;
                            break;
                    }

                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    break;

                case DraggingCardPosition.FinishedOverThreshold:
                    view.SetAppThemeColor(BackgroundColorProperty, lightColor, darkColor);
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
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