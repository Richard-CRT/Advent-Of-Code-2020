using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_15___Rambunctious_Recitation
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<Turn>> numbersTurns = new Dictionary<int, List<Turn>>();
            Turn lastTurn = null;

            List<string> inputList = AoCUtilities.GetInputLines();
            string[] turnStrings = inputList[0].Split(',');
            int turn_i = 1;
            for (; turn_i <= turnStrings.Length; turn_i++)
            {
                string turnString = turnStrings[turn_i - 1];
                int number = int.Parse(turnString);
                Turn turn = new Turn(turn_i, number);
                if (!numbersTurns.ContainsKey(number))
                {
                    numbersTurns[number] = new List<Turn> { turn };
                }
                else
                {
                    numbersTurns[number].Add(turn);
                }
                lastTurn = turn;
            }

            for (; turn_i <= 30000000; turn_i++)
            {
                int lastNumberSpoke = lastTurn.number;
                List<Turn> turnsWithThatNumber = numbersTurns[lastNumberSpoke];
                int nextNumber;
                if (turnsWithThatNumber.Count > 1)
                {
                    int lastTurnThatNumberSpoken = lastTurn.id;
                    int secondLastTurnThatNumberSpoken = turnsWithThatNumber[turnsWithThatNumber.Count - 2].id;
                    nextNumber = lastTurnThatNumberSpoken - secondLastTurnThatNumberSpoken;
                }
                else
                {
                    nextNumber = 0;
                }

                if (turn_i == 2020)
                    Console.WriteLine(nextNumber);

                Turn newTurn = new Turn(turn_i, nextNumber);
                if (!numbersTurns.ContainsKey(nextNumber))
                {
                    numbersTurns[nextNumber] = new List<Turn> { newTurn };
                }
                else
                {
                    numbersTurns[nextNumber].Add(newTurn);
                }
                lastTurn = newTurn;
            }

            numbersTurns.Clear();
            Console.WriteLine(lastTurn.number);
            Console.ReadLine();
        }
    }

    class Turn
    {
        public int id;
        public int number;

        public Turn(int id, int number)
        {
            this.id = id;
            this.number = number;
        }
    }
}
