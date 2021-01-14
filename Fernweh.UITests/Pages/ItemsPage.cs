using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace Fernweh.UITests
{
    public class ItemsPage : BasePage
    {
        private readonly Query addToolbarButton;

        public ItemsPage(IApp app, Platform platform) : base(app, platform, "Browse")
        {
            addToolbarButton = x => x.Marked("Add");
        }

        public void TapAddToolbarButton()
        {
            app.Tap(addToolbarButton);

            app.Screenshot("Toolbar Item Tapped");
        }
    }
}