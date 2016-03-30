using System;
using Xamarin.Forms;

namespace MindMaster
{
    public class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            Title = "Welcome";

            Button continueButton = new Button
            {
                BackgroundColor = Color.FromRgb(44, 99, 33),
                Text = "Continue"
            };
            continueButton.Clicked += ContinueButton_Clicked;
            Button aboutButton = new Button
            {
                BackgroundColor = Color.FromRgb(44, 99, 33),
                Text = "About"
            };
            aboutButton.Clicked += AboutButton_Clicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White,
                Padding = 40,
                Spacing = 16,
                Children = {
                    new Label {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "Welcome to MindMaster!",
                        TextColor = Color.FromRgb(44,99,33),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    },
                    new Image
                    {
                        //Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("logo-small-smonly.jpg")

                    },
                    continueButton,
                    aboutButton
                }
            };

            
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new AboutPage());
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new AddWordsPage());
        }
    }
}
