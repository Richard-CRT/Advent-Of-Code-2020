using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_23___Crab_Cups
{
    class Program
    {
        static void Print(LinkedListNode printNode, int count = 9)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{printNode.value}");
                printNode = printNode.next;
            }
            Console.WriteLine();
        }

        static void Sim(int totalCups, int totalTurns, LinkedListNode currentCup, LinkedListNode[] nodesByValue)
        {
            for (int turn_i = 0; turn_i < totalTurns; turn_i++)
            {
                const int cupsToPickUp = 3;
                LinkedListNode firstToRemove = currentCup.next;
                LinkedListNode lastToRemove = firstToRemove.Advance(cupsToPickUp - 1);
                currentCup.next = lastToRemove.next;
                lastToRemove.next.previous = currentCup;
                firstToRemove.previous = null;
                lastToRemove.next = null;

                long trialValueOfDestinationCup = currentCup.value;
                for (long i = 0; i < cupsToPickUp + 1; i++)
                {
                    trialValueOfDestinationCup--;
                    if (trialValueOfDestinationCup == 0)
                        trialValueOfDestinationCup = totalCups;

                    bool present = firstToRemove.value == trialValueOfDestinationCup
                                || firstToRemove.next.value == trialValueOfDestinationCup
                                || lastToRemove.value == trialValueOfDestinationCup;
                    if (!present)
                        break;
                }

                LinkedListNode destinationCup = nodesByValue[trialValueOfDestinationCup];

                LinkedListNode cupAfterDestinationCup = destinationCup.next;
                destinationCup.next = firstToRemove;
                firstToRemove.previous = destinationCup;
                lastToRemove.next = cupAfterDestinationCup;
                cupAfterDestinationCup.previous = lastToRemove;

                currentCup = currentCup.next;
            }
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            //////////////////////////////////////
            // Part 1
            //////////////////////////////////////

            const int totalCupsP1 = 9;

            LinkedListNode[] nodesByValue = new LinkedListNode[totalCupsP1 + 1];
            LinkedListNode previousNode = null;
            LinkedListNode firstNode = null;

            foreach (char c in inputList[0])
            {
                int value = (int)char.GetNumericValue(c);
                LinkedListNode newNode = new LinkedListNode(value);
                nodesByValue[value] = newNode;
                if (firstNode == null)
                    firstNode = newNode;
                if (previousNode != null)
                {
                    newNode.previous = previousNode;
                    previousNode.next = newNode;
                }
                previousNode = newNode;
            }

            firstNode.previous = previousNode;
            previousNode.next = firstNode;

            LinkedListNode currentCup = firstNode;

            Sim(totalCupsP1, 100, currentCup, nodesByValue);

            LinkedListNode oneNode = nodesByValue[1];
            Print(oneNode.next, 8);

            //////////////////////////////////////
            // Part 2
            //////////////////////////////////////

            const int totalCupsP2 = 1000000;

            nodesByValue = new LinkedListNode[totalCupsP2 + 1];
            previousNode = null;
            firstNode = null;

            foreach (char c in inputList[0])
            {
                long value = (long)char.GetNumericValue(c);
                LinkedListNode newNode = new LinkedListNode(value);
                nodesByValue[value] = newNode;
                if (firstNode == null)
                    firstNode = newNode;
                if (previousNode != null)
                {
                    newNode.previous = previousNode;
                    previousNode.next = newNode;
                }
                previousNode = newNode;
            }

            for (long i = 10; i <= totalCupsP2; i++)
            {
                LinkedListNode newNode = new LinkedListNode(i);
                nodesByValue[i] = newNode;
                newNode.previous = previousNode;
                previousNode.next = newNode;
                previousNode = newNode;
            }

            firstNode.previous = previousNode;
            previousNode.next = firstNode;

            currentCup = firstNode;

            Sim(totalCupsP2, 10000000, currentCup, nodesByValue);

            oneNode = nodesByValue[1];

            long product = oneNode.next.value * oneNode.next.next.value;
            Console.WriteLine(product);

            Console.ReadLine();
        }
    }

    class LinkedListNode
    {
        public LinkedListNode previous;
        public LinkedListNode next;
        public long value;

        public LinkedListNode(long value)
        {
            this.value = value;
        }

        public LinkedListNode Advance(int positions)
        {
            LinkedListNode ret = this;
            for (int i = 0; i < positions; i++)
                ret = ret.next;
            return ret;
        }

        public override string ToString()
        {
            return $"{value}";
        }
    }
}
