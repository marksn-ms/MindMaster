using Xamarin.Forms;

namespace MindMaster
{
    public class AboutPage : ContentPage
    {
        public AboutPage()
        {
            Title = "About";
            Content = new StackLayout
            {
                Padding = 40,
                Children = {
                    new Label {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "MindMaster!",
                        TextColor = Color.FromRgb(44,99,33),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    },
                    new Label
                    {
                        TextColor = Color.FromRgb(44,99,33),
                        Text = "This is a tool to help you survive a game against a machine that presents a list of words and only allows you a few guesses to choose the correct word.  When you guess a word, the machine will tell you how many correct letters your word has in the correct position.  If you provide the list of words this tool can tell you which words to guess in the (provably) fewest tries."
                    },
                    new Label
                    {
                        TextColor = Color.FromRgb(44,99,33),
                        Text = "Good luck!"
                    }
                }
            };
        }
    }
}
