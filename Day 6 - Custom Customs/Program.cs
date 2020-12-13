using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_6___Custom_Customs
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = AoCUtilities.GetInput();
            List<string> groupStrings = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<Group> groups = groupStrings.ConvertAll(str => new Group(str));

            int summedUniqueAnswers = 0;
            foreach (Group group in groups)
            {
                summedUniqueAnswers += group.uniqueAnswerCount;
            }
            int summedEveryoneAnsweredYes = 0;
            foreach (Group group in groups)
            {
                summedEveryoneAnsweredYes += group.everyoneAnsweredYesCount;
            }

            Console.WriteLine(summedUniqueAnswers);
            Console.WriteLine(summedEveryoneAnsweredYes);
            Console.ReadLine();
        }
    }

    class Group
    {
        Dictionary<char, int> answers = new Dictionary<char, int>();
        int peopleInGroupCount = 0;
        public int uniqueAnswerCount
        {
            get
            {
                return answers.Count;
            }
        }
        public int everyoneAnsweredYesCount
        {
            get
            {
                int everyoneAnsweredYesCount = 0;
                foreach (var kvPair in this.answers)
                {
                    if (kvPair.Value == this.peopleInGroupCount)
                    {
                        everyoneAnsweredYesCount++;
                    }
                }
                return everyoneAnsweredYesCount;
            }
        }

        public Group(string groupString)
        {
            string[] groupParts = groupString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            peopleInGroupCount = groupParts.Length;
            foreach (char character in groupString)
            {
                if ((int)character >= 0x61 && (int)character <= 0x7A)
                {
                    if (answers.ContainsKey(character))
                    {
                        answers[character]++;
                    }
                    else
                    {
                        answers[character] = 1;
                    }
                }
            }
        }
    }
}
