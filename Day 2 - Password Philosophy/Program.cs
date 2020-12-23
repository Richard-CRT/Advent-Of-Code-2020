using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_2___Password_Philosophy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            List<Password> passwords = inputList.ConvertAll(str => new Password(str));
            int validP1PasswordsCount = passwords.Count(password => password.validP1);
            int validP2PasswordsCount = passwords.Count(password => password.validP2);

            Console.WriteLine(validP1PasswordsCount);
            Console.WriteLine(validP2PasswordsCount);
            Console.ReadLine();
        }
    }

    class Password
    {
        string password;
        char restrictedLetter;
        int rangeArg1;
        int rangeArg2;

        public bool validP1;
        public bool validP2;

        public Password(string inputLine)
        {
            string[] inputParts = inputLine.Split(' ');
            this.password = inputParts[2];
            this.restrictedLetter = inputParts[1][0];
            string[] rangeParts = inputParts[0].Split('-');
            this.rangeArg1 = int.Parse(rangeParts[0]);
            this.rangeArg2 = int.Parse(rangeParts[1]);

            this.CheckValidityP1();
            this.CheckValidityP2();
        }

        public void CheckValidityP1()
        {
            int restrictedCharOccurances = 0;
            foreach (char character in this.password)
            {
                if (character == this.restrictedLetter)
                {
                    restrictedCharOccurances++;
                    if (restrictedCharOccurances > this.rangeArg2)
                    {
                        break;
                    }
                }
            }
            this.validP1 = restrictedCharOccurances >= this.rangeArg1 && restrictedCharOccurances <= this.rangeArg2;
        }

        public void CheckValidityP2()
        {
            this.validP2 = (this.password[rangeArg1-1] == this.restrictedLetter ^ this.password[rangeArg2 - 1] == this.restrictedLetter);
        }
    }
}
