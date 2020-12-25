using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_25___Combo_Breaker
{
    class Program
    {
        static long Transform(int loopSize, int subjectNumber, long startVal, int startLoopSize)
        {
            long value = startVal;
            for (int i = startLoopSize; i <= loopSize; i++)
            {
                value = value * subjectNumber;
                if (value > 20201227)
                    value = value % 20201227;
            }
            return value;
        }

        static long Transform(int loopSize, int subjectNumber)
        {
            return Transform(loopSize, subjectNumber, 1, 1);
        }

        static int FindLoopSize(int publicKey, int subjectNumber)
        {
            int loopSize = 0;
            long value = 1;
            while (value != publicKey)
            {
                loopSize++;
                value = Transform(loopSize, subjectNumber, value, loopSize);
            }
            return loopSize;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            int cardPublicKey = int.Parse(inputList[0]);
            int doorPublicKey = int.Parse(inputList[1]);

            //cardPublicKey = 5764801;
            //doorPublicKey = 17807724;

            int cardLoopSize = FindLoopSize(cardPublicKey, 7);
            int doorLoopSize = FindLoopSize(doorPublicKey, 7);

            long encryptionKey1 = Transform(cardLoopSize, doorPublicKey);
            long encryptionKey2 = Transform(doorLoopSize, cardPublicKey);

            if (encryptionKey1 != encryptionKey2)
                throw new ArgumentException("Invalid input");

            Console.WriteLine(encryptionKey1);
            Console.ReadLine();
        }
    }
}
