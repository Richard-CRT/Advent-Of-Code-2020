using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_16___Ticket_Translation
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            List<Field> fields = new List<Field>();
            int i = 0;
            string line = inputList[i];
            while (line != "")
            {
                var parts1 = line.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                string name = parts1[0];
                var field = new Field(name);

                var parts2 = parts1[1].Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string requirementString in parts2)
                {
                    var requirementStringParts = requirementString.Split('-');
                    int min = int.Parse(requirementStringParts[0]);
                    int max = int.Parse(requirementStringParts[1]);
                    field.requirements.Add(new Requirement(min, max));
                }

                fields.Add(field);

                i++;
                line = inputList[i];
            }

            i += 2;
            string yourTicketString = inputList[i];
            Ticket yourTicket = new Ticket(yourTicketString);

            List<Ticket> otherTickets = new List<Ticket>();
            i += 3;
            while (i < inputList.Count)
            {
                line = inputList[i];

                otherTickets.Add(new Ticket(line));

                i++;
            }

            List<Ticket> validOtherTickets = new List<Ticket>();

            int sumP1 = 0;
            foreach (Ticket otherTicket in otherTickets)
            {
                bool allValuesValid = true;
                foreach (int value in otherTicket.values)
                {
                    bool valid = false;
                    foreach (Field field in fields)
                    {
                        foreach (Requirement requirement in field.requirements)
                        {
                            if (value >= requirement.min && value <= requirement.max)
                            {
                                valid = true;
                                break;
                            }
                        }
                        if (valid)
                            break;
                    }
                    if (!valid)
                    {
                        sumP1 += value;
                        allValuesValid = false;
                    }
                }
                otherTicket.valid = allValuesValid;
                if (allValuesValid)
                    validOtherTickets.Add(otherTicket);
            }

            Console.WriteLine(sumP1);

            foreach (Field field in fields)
            {
                for (int x = 0; x < yourTicket.values.Count; x++)
                {
                    field.possiblePositions.Add(x);
                }
            }

            foreach (Field field in fields)
            {
                for (int possiblePosition_i = 0; possiblePosition_i < field.possiblePositions.Count;)
                {
                    int possiblePosition = field.possiblePositions[possiblePosition_i];

                    bool positionValid = true;
                    foreach (Ticket ticket in validOtherTickets)
                    {
                        int trialValue = ticket.values[possiblePosition];
                        bool valid = false;
                        foreach (Requirement requirement in field.requirements)
                        {
                            if (trialValue >= requirement.min && trialValue <= requirement.max)
                            {
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            positionValid = false;
                        }
                    }

                    if (!positionValid)
                        field.possiblePositions.RemoveAt(possiblePosition_i);
                    else
                        possiblePosition_i++;
                }
            }

            bool changeMade = true;
            while (changeMade)
            {
                changeMade = false;

                foreach (Field field in fields)
                {
                    if (field.position == -1 && field.possiblePositions.Count == 1)
                    {
                        int knownPositionForThisField = field.possiblePositions[0];
                        field.position = knownPositionForThisField;
                        // remove this position from other fields
                        foreach (Field otherField in fields)
                        {
                            if (field != otherField)
                            {
                                changeMade = true;
                                otherField.possiblePositions.Remove(knownPositionForThisField);
                            }
                        }
                    }
                }
            }

            bool foundSolution = true;
            long product = 1;
            foreach (Field field in fields)
            {
                if (field.position == -1)
                {
                    foundSolution = false;
                    break;
                }

                if (field.name.Length >= 9 && field.name.Substring(0,9) == "departure")
                {
                    product *= yourTicket.values[field.position];
                }
            }

            if (!foundSolution)
            {
                throw new Exception("No solution found");
            }

            Console.WriteLine(product);
            Console.ReadLine();
        }
    }

    class Ticket
    {
        public string stringDesc;
        public List<int> values = new List<int>();
        public bool valid = true;

        public Ticket(string stringDesc)
        {
            this.stringDesc = stringDesc;
            var stringDescParts = stringDesc.Split(',');
            foreach (string stringDescPart in stringDescParts)
            {
                this.values.Add(int.Parse(stringDescPart));
            }
        }
    }

    class Field
    {
        public string name;
        public List<Requirement> requirements = new List<Requirement>();
        public List<int> possiblePositions = new List<int>();
        public int position = -1;

        public Field(string name)
        {
            this.name = name;
        }
    }

    class Requirement
    {
        public int min;
        public int max;

        public Requirement(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
