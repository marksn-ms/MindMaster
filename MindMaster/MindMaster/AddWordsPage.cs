using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MindMaster
{
    public class AddWordsPage : ContentPage
    {
        protected Entry addWordEntry;
        protected Button solveGameButton;
        protected ObservableCollection<string> wordList = new ObservableCollection<string>();
        protected ExactLengthWordValidatorBehavior wordLengthValidator = new ExactLengthWordValidatorBehavior(0);
        
        public AddWordsPage()
        {
            Title = "Add words";

            Button addWordButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromRgb(44, 99, 33),
                Text = "Add"
            };
            addWordButton.Clicked += AddButton_Clicked;

            ListView wordListView = new ListView
            {
                ItemsSource = wordList
            };
            wordListView.ItemTapped += WordListView_ItemTapped;

            addWordEntry = new Entry()
            {
                Placeholder = "Enter a word",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Text
            };
            addWordEntry.Behaviors.Add(wordLengthValidator);

            solveGameButton = new Button()
            {
                Text = "Solve",
                IsEnabled = false
            };
            solveGameButton.Clicked += SolveGameButton_Clicked;

            Content = new ScrollView() {
                Content = new StackLayout()
                {
                    Padding = new Thickness(20, 0),
                    Children =
                    {
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { addWordEntry, addWordButton }
                        },
                        solveGameButton,
                        wordListView
                    }
                }
            };
        }

        private void SolveGameButton_Clicked(object sender, EventArgs e)
        {
            // push next page on stack
            this.Navigation.PushAsync(new SolverPage(wordList));
        }

        private void WordListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            wordList.Remove(e.Item as string);
            if (wordList.Count == 0)
                wordLengthValidator.ExactLength = 0; // reset the length so any length works again
            solveGameButton.IsEnabled = (wordList.Count > 1);
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            string addWordText = addWordEntry.Text?.Trim();
            if ((addWordText != null && addWordText.Length > 0)
                && ((wordList.Count > 0 && wordList[0].Length == addWordText.Length) || (wordList.Count == 0)))
            {
                wordLengthValidator.ExactLength = addWordText.Length;
                wordList.Add(addWordText);
                addWordEntry.Text = "";
            }
            solveGameButton.IsEnabled = (wordList.Count > 1);
        }
    }
}
