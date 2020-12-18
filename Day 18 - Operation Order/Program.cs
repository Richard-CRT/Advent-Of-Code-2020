using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_18___Operation_Order
{
    class Program
    {
        static int OperatorPositionP1(string expression)
        {
            int lastOperatorPos = -1;
            int bracketRecursionLevel = 0;
            for (int character_i = expression.Length - 1; character_i >= 0; character_i--)
            {
                char character = expression[character_i];
                if (character == ')')
                    bracketRecursionLevel++;
                if (character == '(')
                    bracketRecursionLevel--;
                if (bracketRecursionLevel == 0)
                {
                    if (character == '*')
                    {
                        lastOperatorPos = character_i;
                        break;
                    }
                    else if (character == '+')
                    {
                        lastOperatorPos = character_i;
                        break;
                    }
                }
            }
            return lastOperatorPos;
        }
        static int OperatorPositionP2(string expression)
        {
            char[] operatorCharList = new char[] { '*', '+' };
            int operatorPos = -1;
            foreach (char operatorChar in operatorCharList)
            {
                int bracketRecursionLevel = 0;
                for (int character_i = expression.Length - 1; character_i >= 0; character_i--)
                {
                    char character = expression[character_i];
                    if (character == ')')
                        bracketRecursionLevel++;
                    if (character == '(')
                        bracketRecursionLevel--;
                    if (bracketRecursionLevel == 0)
                    {
                        if (character == operatorChar)
                        {
                            operatorPos = character_i;
                            break;
                        }
                    }
                }
                if (operatorPos != -1)
                    break;
            }
            return operatorPos;
        }

        static long Evaluate(string expression, bool part2 = false)
        {
            long result = 0;

            // find last operator in this scope (ignorning contents of brackets)

            int operatorPos;
            if (!part2)
                operatorPos = OperatorPositionP1(expression);
            else
                operatorPos = OperatorPositionP2(expression);

            if (operatorPos != -1)
            {
                string leftExpression = expression.Substring(0, operatorPos - 1);
                string rightExpression = expression.Substring(operatorPos + 2);
                long left = Evaluate(leftExpression, part2);
                long right = Evaluate(rightExpression, part2);
                if (expression[operatorPos] == '*')
                    result = left * right;
                else if (expression[operatorPos] == '+')
                    result = left + right;
            }
            else
            {
                if (expression[0] == '(' && expression[expression.Length - 1] == ')')
                {
                    expression = expression.Substring(1, expression.Length - 2);
                    result = Evaluate(expression, part2);
                }
                else
                {
                    result = long.Parse(expression);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            // Part 1
            long sumP1 = 0;
            foreach (string expression in inputList)
            {
                long result = Evaluate(expression);
                sumP1 += result;
            }

            Console.WriteLine(sumP1);

            // Part 2
            long sumP2 = 0;
            foreach (string expression in inputList)
            {
                long result = Evaluate(expression, true);
                sumP2 += result;
            }

            Console.WriteLine(sumP2);
            Console.ReadLine();
        }
    }

    public enum Operator { multiply, add };
}
