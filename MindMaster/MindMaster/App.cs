
using Xamarin.Forms;

namespace MindMaster
{
    public class App : Application
    {
        public App()
        {
            Page welcomePage = new WelcomePage();

            MainPage = new NavigationPage(welcomePage);

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
