using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_9___Encoding_Error
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            List<long> inputNumbers = inputList.ConvertAll(str => long.Parse(str));
            const int preambleLength = 25;
            List<long> availableNumbers = new List<long>(preambleLength);
            for (int i = 0; i < preambleLength; i++)
            {
                availableNumbers.Add(inputNumbers[i]);
            }

            int number_i = preambleLength;
            for (; number_i < inputNumbers.Count; number_i++)
            {
                long targetNumber = inputNumbers[number_i];
                bool foundSum = false;
                foreach (long availableNumber1 in availableNumbers)
                {
                    foreach (long availableNumber2 in availableNumbers)
                    {
                        if (availableNumber1 + availableNumber2 == targetNumber)
                        {
                            foundSum = true;
                            break;
                        }
                    }
                    if (foundSum)
                        break;
                }
                if (foundSum)
                {
                    availableNumbers.RemoveAt(0);
                    availableNumbers.Add(targetNumber);
                }
                else
                {
                    break;
                }
            }

            long badNumber = inputNumbers[number_i];
            Console.WriteLine(badNumber);

            List<long> contiguousRange = null;
            for (int startNumber_i = 0; startNumber_i < inputNumbers.Count - 1; startNumber_i++)
            {
                long sum = 0;
                int contiguousElementCount = 2;
                while (sum < badNumber)
                {
                    sum = inputNumbers.Skip(startNumber_i).Take(contiguousElementCount).Sum();
                    contiguousElementCount++;
                }
                if (sum == badNumber)
                {
                    contiguousRange = inputNumbers.Skip(startNumber_i).Take(contiguousElementCount - 1).ToList();
                }
            }
            if (contiguousRange == null)
            {
                throw new ArgumentException("No contiguous range is found that sums to the value " + badNumber);
            }
            long minInContiguousRange = contiguousRange.Min();
            long maxInContiguousRange = contiguousRange.Max();
            long weakness = minInContiguousRange + maxInContiguousRange;
            Console.WriteLine(weakness);
            Console.ReadLine();
        }
    }
}
