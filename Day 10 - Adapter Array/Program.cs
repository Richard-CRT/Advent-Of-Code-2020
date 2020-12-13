using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_10___Adapter_Array
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> joltageDifferences = new Dictionary<int, int>();

            List<string> inputList = AoCUtilities.GetInputLines();
            List<int> availableAdapters = inputList.ConvertAll(str => int.Parse(str));

            List<int> sortedAdapters = availableAdapters.OrderBy(adapter => adapter).ToList();

            joltageDifferences[3] = 1;
            for (int adapter_i = 0; adapter_i < sortedAdapters.Count; adapter_i++)
            {
                int adapterJoltage = sortedAdapters[adapter_i];
                int joltageDifference;
                if (adapter_i == 0)
                {
                    joltageDifference = adapterJoltage;
                }
                else
                {
                    joltageDifference = adapterJoltage - sortedAdapters[adapter_i - 1];
                }

                if (joltageDifferences.ContainsKey(joltageDifference))
                {
                    joltageDifferences[joltageDifference]++;
                }
                else
                {
                    joltageDifferences[joltageDifference] = 1;
                }
            }

            Console.WriteLine(joltageDifferences[1] * joltageDifferences[3]);


            Dictionary<int, Node> adapterNodes = new Dictionary<int, Node>();

            Node outletNode = new Node(0);
            adapterNodes[0] = outletNode;
            outletNode.WaysToReach = 1;

            int highestJoltageAdapter = sortedAdapters.Max();
            Node deviceNode = new Node(highestJoltageAdapter+3);
            adapterNodes[highestJoltageAdapter+3] = deviceNode;

            for (int adapter_i = 0; adapter_i < sortedAdapters.Count; adapter_i++)
            {
                int adapterJoltage = sortedAdapters[adapter_i];
                adapterNodes[adapterJoltage] = new Node(adapterJoltage);
            }

            outletNode.FindNodesInReach(adapterNodes);
            for (int adapter_i = 0; adapter_i < sortedAdapters.Count; adapter_i++)
            {
                int adapterJoltage = sortedAdapters[adapter_i];
                adapterNodes[adapterJoltage].FindNodesInReach(adapterNodes);
            }

            Console.WriteLine(deviceNode.WaysToReach);
            Console.ReadLine();
        }
    }

    class Node
    {
        public int Joltage = 0;
        public long WaysToReach = 0;
        public List<Node> NodesInReach = new List<Node>();

        public Node(int joltage)
        {
            this.Joltage = joltage;
        }

        public void FindNodesInReach(Dictionary<int, Node> adapterNodes)
        {
            foreach (var kv in adapterNodes)
            {
                Node otherAdapterNode = kv.Value;
                int otherJoltage = otherAdapterNode.Joltage;
                int joltageDifference = otherJoltage - this.Joltage;
                if (joltageDifference >= 1 && joltageDifference <= 3)
                {
                    this.NodesInReach.Add(kv.Value);
                    otherAdapterNode.WaysToReach += this.WaysToReach;
                }
            }
        }
    }
}
