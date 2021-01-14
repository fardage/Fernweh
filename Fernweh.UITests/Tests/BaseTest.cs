using NUnit.Framework;
using Xamarin.UITest;

namespace Fernweh.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        [SetUp]
        public virtual void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            app.Screenshot("App Initialized");

            ItemsPage = new ItemsPage(app, platform);
            NewItemPage = new NewItemPage(app, platform);
        }

        protected IApp app;
        protected Platform platform;

        protected ItemsPage ItemsPage;
        protected NewItemPage NewItemPage;

        protected BaseTest(Platform platform)
        {
            this.platform = platform;
        }
    }
}