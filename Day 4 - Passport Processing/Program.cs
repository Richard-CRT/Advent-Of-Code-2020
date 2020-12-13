using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_4___Passport_Processing
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = AoCUtilities.GetInput();
            List<string> passportStrings = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None).ToList();
            List<Passport> passports = passportStrings.ConvertAll(str => new Passport(str));
            int validPassportsCount = passports.Count(passport => passport.valid);

            Console.WriteLine(validPassportsCount);
            Console.ReadLine();
        }
    }

    class Passport
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        public bool valid = true;

        public Passport(string passportString)
        {
            string[] passportParts = passportString.Split(new string[] { "\r\n", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string passportPart in passportParts)
            {
                string[] entryParts = passportPart.Split(':');
                data[entryParts[0]] = entryParts[1];
            }

            this.CheckValidity();
        }

        public void CheckValidity()
        {
            int byr = 0;
            this.valid = this.valid && this.data.ContainsKey("byr") && int.TryParse(data["byr"], out byr) && byr >= 1920 && byr <= 2002;
            int iyr = 0;
            this.valid = this.valid && this.data.ContainsKey("iyr") && int.TryParse(data["iyr"], out iyr) && iyr >= 2010 && iyr <= 2020;
            int eyr = 0;
            this.valid = this.valid && this.data.ContainsKey("eyr") && int.TryParse(data["eyr"], out eyr) && eyr >= 2020 && eyr <= 2030;

            this.valid = this.valid && this.data.ContainsKey("hgt") && data["hgt"].Length >= 4;
            if (this.valid)
            {
                string last2Char = data["hgt"].Substring(data["hgt"].Length - 2);
                int param;
                if (last2Char == "cm")
                {
                    this.valid = this.valid && int.TryParse(data["hgt"].Substring(0, 3), out param) && param >= 150 && param <= 193;
                }
                else if (last2Char == "in")
                {
                    this.valid = this.valid && int.TryParse(data["hgt"].Substring(0, 2), out param) && param >= 59 && param <= 76;
                }
                else
                {
                    this.valid = false;
                }
            }

            this.valid = this.valid && this.data.ContainsKey("hcl") && data["hcl"].Length == 7 && data["hcl"][0] == '#';
            char[] validHCLCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            for (int i = 0; i < 6; i++)
            {
                this.valid = this.valid && validHCLCharacters.Contains(data["hcl"][i + 1]);
            }

            this.valid = this.valid && this.data.ContainsKey("ecl") && (
                data["ecl"] == "amb" ||
                data["ecl"] == "blu" ||
                data["ecl"] == "brn" ||
                data["ecl"] == "gry" ||
                data["ecl"] == "grn" ||
                data["ecl"] == "hzl" ||
                data["ecl"] == "oth"
                );

            this.valid = this.valid && this.data.ContainsKey("pid") && data["pid"].Length == 9;
        }
    }
}
