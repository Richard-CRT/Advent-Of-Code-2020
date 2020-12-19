using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_19___Monster_Messages
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Rule> rules = new Dictionary<int, Rule>();

            string input = AoCUtilities.GetInput();
            var inputParts = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string rulesString = inputParts[0];
            string[] ruleStrings = rulesString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string messages = inputParts[1];
            string[] messageStrings = messages.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string ruleString in ruleStrings)
            {
                var ruleStringParts = ruleString.Split(':');
                int id = int.Parse(ruleStringParts[0]);

                Rule newRule;
                if (rules.ContainsKey(id))
                    newRule = rules[id];
                else
                {
                    newRule = new Rule();
                    newRule.id = id;
                    rules[id] = newRule;
                }

                if (ruleStringParts[1][1] == '"')
                {
                    newRule.leaf = true;
                    newRule.value = ruleStringParts[1][2];
                }
                else
                {
                    var ruleStringAlternatives = ruleStringParts[1].Split('|');
                    foreach (string ruleStringAlternative in ruleStringAlternatives)
                    {
                        string[] subRuleStrings = ruleStringAlternative.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        StringOfRules stringOfRules = new StringOfRules();
                        foreach (string subRuleString in subRuleStrings)
                        {
                            int subRuleId = int.Parse(subRuleString);
                            Rule subRule;
                            if (rules.ContainsKey(subRuleId))
                                subRule = rules[subRuleId];
                            else
                            {
                                subRule = new Rule();
                                subRule.id = subRuleId;
                                rules[subRuleId] = subRule;
                            }
                            stringOfRules.rules.Add(subRule);
                        }

                        newRule.alternatives.Add(stringOfRules);
                    }
                }
            }

            string regex = rules[0].EvaluateRegex();
            string regexPatternP1 = $@"^\b{regex}\b$";
            AoCUtilities.DebugWriteLine("{0}", regexPatternP1.Length);
            AoCUtilities.DebugWriteLine(regexPatternP1);
            Regex regexP1 = new Regex(regexPatternP1);
            int messagesMatchCountP1 = 0;
            foreach (string messageString in messageStrings)
            {
                bool match = regexP1.IsMatch(messageString);
                if (match)
                    messagesMatchCountP1++;
            }
            Console.WriteLine(messagesMatchCountP1);

            var new8strOfRules = new StringOfRules();
            new8strOfRules.rules.Add(rules[42]);
            new8strOfRules.rules.Add(rules[8]);
            rules[8].alternatives.Add(new8strOfRules);
            var new11strOfRules = new StringOfRules();
            new11strOfRules.rules.Add(rules[42]);
            new11strOfRules.rules.Add(rules[11]);
            new11strOfRules.rules.Add(rules[31]);
            rules[11].alternatives.Add(new11strOfRules);

            regex = rules[0].EvaluateRegex();
            string regexPatternP2 = $@"^\b{regex}\b$";
            AoCUtilities.DebugWriteLine("{0}", regexPatternP2.Length);
            AoCUtilities.DebugWriteLine(regexPatternP2);
            Regex regexP2 = new Regex(regexPatternP2);
            int messagesMatchCountP2 = 0;
            foreach (string messageString in messageStrings)
            {
                bool match = regexP2.IsMatch(messageString);
                if (match)
                    messagesMatchCountP2++;
            }
            Console.WriteLine(messagesMatchCountP2);

            Console.ReadLine();
        }
    }

    class Rule
    {
        public int id;
        public bool leaf = false;
        public char value;

        public List<StringOfRules> alternatives = new List<StringOfRules>();

        public string EvaluateRegex(int recursion = 0)
        {
            string regex = "";
            if (this.leaf)
                regex += value;
            else
            {
                regex += "(";
                foreach (StringOfRules stringOfRules in alternatives)
                {
                    foreach (Rule rule in stringOfRules.rules)
                    {
                        if (recursion < 5 || rule.id != this.id)
                        {
                            regex += rule.EvaluateRegex(recursion + 1);
                        }
                    }
                    regex += "|";
                }
                regex = regex.Substring(0, regex.Length - 1);
                regex += ")";
            }
            return regex;
        }


        // This doesn't work, was an attempt to avoid using regex
        public int CheckMessage(string messageSubstring, int recursion = 0)
        {
            //for (int i = 0; i < recursion; i++)
            //    Console.Write("\t");
            //Console.WriteLine($"{this.id} {messageSubstring}");
            int lengthOfMatch = 0;
            if (this.leaf)
            {
                if (messageSubstring.Length > 0 && messageSubstring[0] == this.value)
                {
                    lengthOfMatch = 1;
                }
            }
            else
            {
                bool matchStr;
                foreach (StringOfRules strOfRules in this.alternatives)
                {
                    for (int i = 0; i < recursion; i++)
                        AoCUtilities.DebugWrite("\t");
                    AoCUtilities.DebugWriteLine($"{messageSubstring} must match {strOfRules}");
                    lengthOfMatch = 0;
                    matchStr = true;
                    foreach (Rule rule in strOfRules.rules)
                    {
                        for (int i = 0; i < recursion; i++)
                            AoCUtilities.DebugWrite("\t");
                        AoCUtilities.DebugWriteLine($"{rule}");
                        string messageSubstringSection = messageSubstring.Substring(lengthOfMatch);
                        int lengthOfSubMatch = rule.CheckMessage(messageSubstringSection, recursion + 1);
                        if (lengthOfSubMatch == 0)
                        {
                            matchStr = false;
                            break;
                        }
                        else
                        {
                            lengthOfMatch += lengthOfSubMatch;
                        }
                    }
                    if (matchStr)
                    {
                        for (int i = 0; i < recursion; i++)
                            AoCUtilities.DebugWrite("\t");
                        AoCUtilities.DebugWriteLine($"True");
                        break;
                    }
                    else
                    {
                        lengthOfMatch = 0;
                        for (int i = 0; i < recursion; i++)
                            AoCUtilities.DebugWrite("\t");
                        AoCUtilities.DebugWriteLine($"False");
                    }
                }
            }
            return lengthOfMatch;
        }

        public override string ToString()
        {
            return $"ID:{id}";
        }
    }

    class StringOfRules
    {
        public List<Rule> rules = new List<Rule>();

        public override string ToString()
        {
            string str = " ";
            foreach (var rule in rules)
            {
                str += rule.id + " ";
            }
            return str;
        }
    }
}
