using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SandmarkTechnologies.MoveTree;

namespace MoveTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mind Master - guessing the right answer from possible choices\nv1.0, Copyright (c) 2016, Sandmark Technologies\n");
            Console.WriteLine("The first step is to enter the possible answers:");

            // ask for strings until one entered was empty
            var possibleAnswers = GetPossibleAnswers();
            if (possibleAnswers.Count() == 0) // entering no possible answers means exit
                return;

            MoveTree<int, string> moveTree = new MoveTree<int, string>(possibleAnswers);
            
            while(!moveTree.HasEureka)
            {
                string bestMove = moveTree.BestMove();

                Console.Write($"I recommend you guess '{bestMove}'.  What result did you get?\n> ");
                string howCloseText = Console.ReadLine().Trim();
                int howClose = 0;
                while (!Int32.TryParse(howCloseText, out howClose) || !moveTree.IsMoveValid(bestMove, howClose))
                {
                    Console.Write("Sorry, what result did you get (number)?\n>");
                    howCloseText = Console.ReadLine().Trim();
                }

                moveTree.PruneFromMove(bestMove, howClose);
            }
            
            // this should always result in a collection of count==1 and a Eureka?
            Debug.Assert(moveTree.HasEureka);
            Console.WriteLine($"Eureka!  The answer was '{moveTree.BestMove()}'.");
            Console.ReadLine();
        }

        private static List<string> GetPossibleAnswers()
        {
            var answers = new List<string>();
            string answerEntered = Console.ReadLine().Trim();
            while (answerEntered.Length > 0)
            {
                if (answers.Count() > 0 && answerEntered.Length != answers[0].Length)
                    Console.WriteLine("Entry rejected because it is not the same length as previous answers.  Try again.");
                else
                    answers.Add(answerEntered);
                answerEntered = Console.ReadLine().Trim();
            }
            return answers;
        }
    }
}
