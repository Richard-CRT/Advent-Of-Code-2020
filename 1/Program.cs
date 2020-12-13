using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            foreach (var entry1 in inputList)
            {
                int val1 = int.Parse(entry1);
                foreach (var entry2 in inputList)
                {
                    int val2 = int.Parse(entry2);
                    foreach (var entry3 in inputList)
                    {
                        int val3 = int.Parse(entry3);
                        if (val1 + val2 + val3 == 2020)
                        {
                            Console.WriteLine(val1 * val2 * val3);
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
