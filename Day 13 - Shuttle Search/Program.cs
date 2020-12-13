using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_13___Shuttle_Search
{
    class Program
    {
        static long LCM(List<int> values)
        {
            long a = values[0];
            long b;
            if (values.Count == 1)
            {
                return values[0];
            }
            else if (values.Count == 2)
            {
                b = values[1];
            }
            else
            {
                b = LCM(values.GetRange(1, values.Count - 1));
            }

            long lcm = (a * b) / GCD(a, b);
            return lcm;
        }

        static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            int earliestDepartureTime = int.Parse(inputList[0]);
            string[] busStringParts = inputList[1].Split(',');
            List<Bus> buses = new List<Bus>();
            foreach (string busStringPart in busStringParts)
            {
                buses.Add(new Bus(busStringPart));
            }

            List<Bus> busesByEarliest = buses.OrderBy(bus => bus.FindFirstDepartureAfter(earliestDepartureTime)).ToList();
            Bus earliestBus = busesByEarliest[0];
            int earliestBusDepartureTime = earliestBus.FindFirstDepartureAfter(earliestDepartureTime);
            int answer = earliestBus.TimePeriod * (earliestBusDepartureTime - earliestDepartureTime);

            Console.WriteLine(answer);

            bool foundSolution = false;
            List<int> busTimesFound = new List<int>();
            long trialTime = 1;
            long timeIncrement = 1;
            while (!foundSolution)
            {
                foundSolution = true;
                
                for (int bus_i = 0; bus_i < buses.Count; bus_i++)
                {
                    Bus bus = buses[bus_i];
                    if (bus.TimePeriod != 0)
                    {
                        long requiredDepartureTime = trialTime + bus_i;
                        if (requiredDepartureTime % bus.TimePeriod != 0)
                        {
                            foundSolution = false;
                            break;
                        }
                        else if (!busTimesFound.Contains(bus.TimePeriod))
                        {
                            busTimesFound.Add(bus.TimePeriod);
                            timeIncrement = LCM(busTimesFound);
                        }
                    }
                }
                trialTime += timeIncrement;
            }
            trialTime -= timeIncrement;

            Console.WriteLine(trialTime);
            Console.ReadLine();
        }
    }

    class Bus
    {
        public int TimePeriod;

        public Bus(string timePeriodString)
        {
            if (timePeriodString == "x")
                this.TimePeriod = 0;
            else
                this.TimePeriod = int.Parse(timePeriodString);
        }

        public int FindFirstDepartureAfter(int time)
        {
            if (this.TimePeriod == 0)
                return int.MaxValue;
            int firstDepartureTime = (time / this.TimePeriod) * this.TimePeriod;
            if (firstDepartureTime < time)
                firstDepartureTime += this.TimePeriod;
            return firstDepartureTime;
        }
    }
}
