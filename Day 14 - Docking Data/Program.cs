using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_14___Docking_Data
{
    class Program
    {
        static List<ulong> GetPotentialAddressesFrom(ulong address, string maskString)
        {
            char[] addressCharArray = Convert.ToString((long)address, 2).PadLeft(36, '0').ToCharArray();
            for (int i = 0; i < 36; i++)
            {
                char character = maskString[i];
                switch (character)
                {
                    case '1':
                        {
                            addressCharArray[i] = '1';
                        }
                        break;
                    case 'X':
                        {
                            addressCharArray[i] = 'X';
                        }
                        break;
                }
            }

            List<char[]> possibleAddresseCharArrays = new List<char[]>();
            List<char[]> newPossibleAddresseCharArrays = new List<char[]> { addressCharArray };
            while (newPossibleAddresseCharArrays.Count > 0)
            {
                possibleAddresseCharArrays = newPossibleAddresseCharArrays;
                newPossibleAddresseCharArrays = new List<char[]>();

                foreach (var possibleAddress in possibleAddresseCharArrays)
                {
                    int firstX = Array.IndexOf(possibleAddress, 'X');
                    if (firstX != -1)
                    {
                        char[] possibleAddress1 = new char[36];
                        possibleAddress.CopyTo(possibleAddress1, 0);
                        char[] possibleAddress2 = new char[36];
                        possibleAddress.CopyTo(possibleAddress2, 0);
                        possibleAddress1[firstX] = '0';
                        possibleAddress2[firstX] = '1';
                        newPossibleAddresseCharArrays.Add(possibleAddress1);
                        newPossibleAddresseCharArrays.Add(possibleAddress2);
                    }
                }
            }

            List<ulong> possibleAddresses = possibleAddresseCharArrays.ConvertAll(charArray => Convert.ToUInt64(new string(charArray), 2));
            return possibleAddresses;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            ulong pow_2_36 = (ulong)Math.Pow(2, 36);

            Dictionary<ulong, ulong> memoryP1 = new Dictionary<ulong, ulong>();

            ulong maxValue36bit = pow_2_36 - 1;
            ulong andMask = maxValue36bit;
            ulong orMask = 0;
            foreach (string line in inputList)
            {
                string[] lineParts = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                if (lineParts[0] == "mask")
                {
                    andMask = maxValue36bit;
                    orMask = 0;
                    for (int i = 0; i < 36; i++)
                    {
                        char character = lineParts[1][i];
                        switch (character)
                        {
                            case '0':
                                {
                                    andMask &= ~((ulong)1 << (35 - i));
                                }
                                break;
                            case '1':
                                {
                                    orMask |= (ulong)1 << (35 - i);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    string addressString = lineParts[0].Substring(4, lineParts[0].Length - 5);
                    ulong address = ulong.Parse(addressString);
                    ulong preMaskData = ulong.Parse(lineParts[1]);
                    ulong postMaskData = (preMaskData | orMask) & andMask;
                    memoryP1[address] = postMaskData;
                }
            }

            ulong totalP1 = 0;
            foreach (var kv in memoryP1)
            {
                totalP1 += kv.Value;
            }
            Console.WriteLine(totalP1);

            ///////////////////////////////////////
            // Part 2
            ///////////////////////////////////////
            ///
            Dictionary<ulong, ulong> memoryP2 = new Dictionary<ulong, ulong>();

            string maskString = "";
            foreach (string line in inputList)
            {
                string[] lineParts = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                if (lineParts[0] == "mask")
                {
                    maskString = lineParts[1];
                }
                else
                {
                    string addressString = lineParts[0].Substring(4, lineParts[0].Length - 5);
                    ulong address = ulong.Parse(addressString);
                    ulong data = ulong.Parse(lineParts[1]);
                    List<ulong> possibleAddresses = GetPotentialAddressesFrom(address, maskString);
                    foreach (ulong possibleAddress in possibleAddresses)
                    {
                        memoryP2[possibleAddress] = data;
                    }
                }
            }

            ulong totalP2 = 0;
            foreach (var kv in memoryP2)
            {
                totalP2 += kv.Value;
            }
            Console.WriteLine(totalP2);
            Console.ReadLine();
        }
    }

    class UInt36
    {
        UInt64 coreValue;

        public UInt36 MaxValue
        {
            get
            {
                return new UInt36(bitMask);
            }
        }

        private readonly UInt64 bitMask = 0;

        public UInt36(UInt64 value)
        {
            this.coreValue = value & bitMask;
        }
    }
}
