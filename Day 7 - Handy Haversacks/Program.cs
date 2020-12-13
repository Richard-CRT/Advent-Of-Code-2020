using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_7___Handy_Haversacks
{
    public static class Program
    {
        public static Dictionary<string, Bag> bags = new Dictionary<string, Bag>();

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            foreach (string bagRuleString in inputList)
            {
                string[] bagRuleStringParts = bagRuleString.Split(new string[] { " bags contain " }, StringSplitOptions.RemoveEmptyEntries);
                string bagColour = bagRuleStringParts[0];
                if (bags.ContainsKey(bagColour))
                {
                    throw new ArgumentException("Rule already exists for this bag");
                }
                bags[bagColour] = new Bag(bagColour, bagRuleStringParts[1]);
            }

            List<Bag> bagsThatWouldAllowShinyGoldBag = new List<Bag>();
            List<Bag> bagsToCheckForThatWouldBeValid = new List<Bag> { bags["shiny gold"] };
            while (bagsToCheckForThatWouldBeValid.Count > 0)
            {
                List<Bag> nextBagsToCheckForThatWouldBeValid = new List<Bag>();
                foreach (Bag bagToCheckForThatWouldBeValid in bagsToCheckForThatWouldBeValid)
                {
                    foreach (KeyValuePair<string, Bag> kv in bags)
                    {
                        if (kv.Value.allowedBags.ContainsKey(bagToCheckForThatWouldBeValid.colour))
                        {
                            if (!bagsThatWouldAllowShinyGoldBag.Contains(kv.Value))
                                bagsThatWouldAllowShinyGoldBag.Add(kv.Value);
                            nextBagsToCheckForThatWouldBeValid.Add(kv.Value);
                        }
                    }
                }
                bagsToCheckForThatWouldBeValid = nextBagsToCheckForThatWouldBeValid;
            }

            int bagsInsideShinyGoldCount = bags["shiny gold"].BagsInsideThisCount();

            Console.WriteLine(bagsThatWouldAllowShinyGoldBag.Count);
            Console.WriteLine(bagsInsideShinyGoldCount);
            Console.ReadLine();
        }
    }

    public class Bag
    {
        public string colour;
        public Dictionary<string, int> allowedBags = new Dictionary<string, int>();

        public Bag(string colour, string ruleString)
        {
            this.colour = colour;
            if (ruleString != "no other bags.")
            {
                string[] bagsInThisBagParts = ruleString.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string bagInThisBagString in bagsInThisBagParts)
                {
                    string[] bagInThisBagParts = bagInThisBagString.Split(' ');
                    int number = int.Parse(bagInThisBagParts[0]);
                    string bagColour = bagInThisBagParts[1] + " " + bagInThisBagParts[2];

                    allowedBags[bagColour] = number;
                }
            }
        }

        public int BagsInsideThisCount()
        {
            int count = 0;
            foreach (KeyValuePair<string, int> allowedBag in allowedBags)
            {
                count += allowedBag.Value + (allowedBag.Value * Program.bags[allowedBag.Key].BagsInsideThisCount());
            }
            return count;
        }
    }
}
