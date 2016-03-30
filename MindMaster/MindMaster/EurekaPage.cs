using Xamarin.Forms;

namespace MindMaster
{
    public class EurekaPage : ContentPage
    {
        public EurekaPage(string theAnswer)
        {
            Title = "Eureka!";
            Content = new StackLayout
            {
                Padding = 40,
                Children = {
                    new Frame
                    {
                        OutlineColor = Color.FromRgb(44, 99, 33),
                        Content = new Label()
                        {
                            Text = theAnswer,
                            FontSize = 48,
                            TextColor = Color.FromRgb(44, 99, 33),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                        }
                    }
                }
            };
        }
    }
}
