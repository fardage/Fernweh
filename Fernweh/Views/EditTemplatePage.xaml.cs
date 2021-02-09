using System;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class EditTemplatePage : ContentPage
    {
        private readonly EditTemplateViewModel viewModel;

        public EditTemplatePage(EditTemplateViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void Done_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}