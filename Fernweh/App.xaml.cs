using Fernweh.Views;
using Xamarin.Forms;

namespace Fernweh
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}