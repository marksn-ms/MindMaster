using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using MoveTreeLib;
using System.Diagnostics;

namespace MindMaster
{
    // this page will have a suggested pick at the top, then a question ("What distance did you get back?"),
    // then a number picker, then (maybe) a list of past entries and distances.
    public class SolverPage : ContentPage
    {
        protected MoveTreeLib.MoveTree<int, string> moveTree;
        protected Label nextPick;
        protected Stepper nextDistance;
        protected Label nextDistanceLabel;
        protected Button nextPickButton;

        public SolverPage(IEnumerable<string> possibleAnswers)
        {
            moveTree = new MoveTree<int, string>(possibleAnswers);
            Debug.Assert(moveTree.Count > 1); // must have multiple choices or we already have Eureka!

            Title = "Solve";

            string bestMove = moveTree.BestMove();

            nextPick = new Label()
            {
                FontSize = 24,
                Text = $"You should pick '{bestMove}'."
            };

            nextDistanceLabel = new Label()
            {
                Text = "0",
                FontSize = 24,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                
            };
            nextDistance = new Stepper(0.0, bestMove.Length, 0.0, 1.0)
            {
                //HeightRequest = 60,
                //WidthRequest = 120,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromRgb(44, 99, 33)
            };
            nextDistance.ValueChanged += NextDistance_ValueChanged;
            nextPickButton = new Button()
            {
                Text = "Pick",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromRgb(44, 99, 33)
            };
            nextPickButton.Clicked += NextPickButton_Clicked;

            Content = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Padding = new Thickness(20, 0),
                    Children =
                    {
                        nextPick,
                        new Label
                        {
                            Text = "How many letters did we get correct?",
                            FontSize = 24
                        },
                        new StackLayout()
                        {
                            Padding = new Thickness(0, 24),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new Frame()
                                {
                                    WidthRequest = 120,
                                    OutlineColor = Color.FromRgb(44,99,33),
                                    Content = nextDistanceLabel
                                },
                                nextDistance
                            }
                        },
                        nextPickButton
                    }
                }
            };

        }

        private void NextPickButton_Clicked(object sender, EventArgs e)
        {
            // get the next best-move, update the label, and set the distance back to 0
            // for the next pick
            int howClose = Convert.ToInt32(nextDistance.Value);
            string bestMoveSave = moveTree.BestMove();
            moveTree.PruneFromMove(moveTree.BestMove(), howClose);
            if (moveTree.Count == 1)
                bestMoveSave = moveTree.BestMove();
            if (!moveTree.HasEureka && (moveTree.Count > 1)) // More moves to explore?
            {
                nextPick.Text = $"You should pick '{moveTree.BestMove()}'.";
                nextDistance.Value = 0; // that should also update the label automagically
            }
            else
            {
                // insert the eureka page and nuke the rest of the navigation stack
                ClearStackPanelAndNavigateFirst(new EurekaPage(bestMoveSave), Navigation);
            }
        }

        public static void ClearStackPanelAndNavigateFirst(Page firstPageToNavigate, INavigation navigation)
        {
            navigation.InsertPageBefore(firstPageToNavigate, navigation.NavigationStack[1]);
            var existingPages = navigation.NavigationStack.ToList();
            for (int i = 2; i < existingPages.Count; i++)
            {
                navigation.RemovePage(existingPages[i]);
            }
        }

        private void NextDistance_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            // update the nextDistanceLabel with the new value
            nextDistanceLabel.Text = $"{e.NewValue}";
        }
    }
}
