using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_22___Crab_Combat
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = AoCUtilities.GetInput();


            string[] playerDecks = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<int> originalDeck1 = new List<int>();
            List<int> originalDeck2 = new List<int>();

            List<int> currentDeck;
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                    currentDeck = originalDeck1;
                else
                    currentDeck = originalDeck2;

                var deckStringParts = playerDecks[i].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 1; j < deckStringParts.Length; j++)
                {
                    var cardString = deckStringParts[j];
                    currentDeck.Add(int.Parse(cardString));
                }
            }

            //////////////////////////////////////
            // Part 1
            //////////////////////////////////////

            List<int> deck1 = new List<int>(originalDeck1);
            List<int> deck2 = new List<int>(originalDeck2);
            List<string> hashes = new List<string>() { string.Join("", deck1) };

            List<int> winningDeck = null;
            while (winningDeck == null && deck1.Count > 0 && deck2.Count > 0)
            {
                int nextDeck1 = deck1[0];
                int nextDeck2 = deck2[0];
                deck1.RemoveAt(0);
                deck2.RemoveAt(0);
                if (nextDeck1 > nextDeck2)
                {
                    deck1.Add(nextDeck1);
                    deck1.Add(nextDeck2);
                }
                else
                {
                    deck2.Add(nextDeck2);
                    deck2.Add(nextDeck1);
                }
                string newHash = string.Join("", deck1);
                if (hashes.Contains(newHash))
                    winningDeck = deck1;
                else
                    hashes.Add(newHash);
            }

            if (winningDeck == null)
            {
                if (deck1.Count == 0)
                    winningDeck = deck2;
                else
                    winningDeck = deck1;
            }

            int score = 0;
            for (int i = 1; i <= winningDeck.Count; i++)
            {
                int value = winningDeck[winningDeck.Count - i];
                score += value * i;
            }

            Console.WriteLine(score);

            //////////////////////////////////////
            // Part 2
            //////////////////////////////////////
            
            deck1 = new List<int>(originalDeck1);
            deck2 = new List<int>(originalDeck2);

            bool player1Wins = RecursiveGame(deck1, deck2);

            if (player1Wins)
                winningDeck = deck1;
            else
                winningDeck = deck2;

            score = 0;
            for (int i = 1; i <= winningDeck.Count; i++)
            {
                int value = winningDeck[winningDeck.Count - i];
                score += value * i;
            }

            Console.WriteLine(score);

            Console.ReadLine();
        }

        static bool RecursiveGame(List<int> deck1, List<int> deck2, int recursionLevel = 1)
        {
            List<string> hashes = new List<string>() { string.Join("", deck1) };

            int round = 1;

            AoCUtilities.DebugWriteLine();
            AoCUtilities.DebugWriteLine($"=== Game Recursion Level {recursionLevel} ===");

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                AoCUtilities.DebugWriteLine($"- Round {round} (Game {recursionLevel}) -");
                AoCUtilities.DebugWriteLine($"Player 1's deck: {string.Join(", ", deck1)}");
                AoCUtilities.DebugWriteLine($"Player 2's deck: {string.Join(", ", deck2)}");
                int nextDeck1 = deck1[0];
                int nextDeck2 = deck2[0];
                deck1.RemoveAt(0);
                deck2.RemoveAt(0);
                AoCUtilities.DebugWriteLine($"Player 1 plays: {nextDeck1}");
                AoCUtilities.DebugWriteLine($"Player 2 plays: {nextDeck2}");

                if (deck1.Count >= nextDeck1 && deck2.Count >= nextDeck2)
                {
                    AoCUtilities.DebugWriteLine($"Playing a sub-game to determine the winner...");
                    // recurse
                    List<int> deck1Copy = new List<int>(deck1.GetRange(0,nextDeck1));
                    List<int> deck2Copy = new List<int>(deck2.GetRange(0, nextDeck2));
                    bool player1Wins = RecursiveGame(deck1Copy, deck2Copy, recursionLevel + 1);
                    if (player1Wins)
                    {
                        deck1.Add(nextDeck1);
                        deck1.Add(nextDeck2);
                        AoCUtilities.DebugWriteLine($"Player 1 wins round {round} of game {recursionLevel}!");
                    }
                    else
                    {
                        deck2.Add(nextDeck2);
                        deck2.Add(nextDeck1);
                        AoCUtilities.DebugWriteLine($"Player 2 wins round {round} of game {recursionLevel}!");
                    }
                }
                else
                {
                    if (nextDeck1 > nextDeck2)
                    {
                        deck1.Add(nextDeck1);
                        deck1.Add(nextDeck2);
                        AoCUtilities.DebugWriteLine($"Player 1 wins round {round} of game {recursionLevel}!");
                    }
                    else
                    {
                        deck2.Add(nextDeck2);
                        deck2.Add(nextDeck1);
                        AoCUtilities.DebugWriteLine($"Player 2 wins round {round} of game {recursionLevel}!");
                    }
                }

                string newHash = string.Join(",", deck1) + "|" + string.Join(",",deck2);
                if (hashes.Contains(newHash))
                {
                    AoCUtilities.DebugWriteLine($"Back to game level {recursionLevel - 1}...");
                    return true;
                }
                else
                    hashes.Add(newHash);

                round++;
                AoCUtilities.DebugWriteLine();
            }

            AoCUtilities.DebugWriteLine($"Back to game level {recursionLevel-1}...");
            return deck2.Count == 0;
        }
    }
}
