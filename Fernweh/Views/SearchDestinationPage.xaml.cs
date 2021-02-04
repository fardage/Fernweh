using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class SearchDestinationPage : ContentPage
    {
        private readonly SearchDestinationViewModel viewModel;
        public SearchDestinationPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new SearchDestinationViewModel();
        }

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagingCenter.Send(this, "ItemSelected", e.Item.ToString());
            Navigation.PopModalAsync();
        }

        void Cancel_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}