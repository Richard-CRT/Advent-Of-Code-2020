using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_8___Handheld_Halting
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            int acc;
            Simulate(inputList, out acc);
            Console.WriteLine(acc);

            bool loop = true;
            for (int instruction_i = 0; instruction_i < inputList.Count; instruction_i++)
            {
                string instruction = inputList[instruction_i];
                string opcode = instruction.Substring(0, 3);
                if (opcode == "nop" || opcode == "jmp")
                {
                    List<string> copiedInputList = new List<string>(inputList);
                    if (opcode == "nop")
                    {
                        copiedInputList[instruction_i] = instruction.Replace("nop", "jmp");
                    }
                    else if (opcode == "jmp")
                    {
                        copiedInputList[instruction_i] = instruction.Replace("jmp", "nop");
                    }
                    loop = Simulate(copiedInputList, out acc);
                    if (!loop)
                    {
                        break;
                    }
                }
            }

            if (loop)
            {
                throw new ArgumentException("Didn't find an instruction to switch that fixes the error");
            }

            Console.WriteLine(acc);
            Console.ReadLine();
        }

        static bool Simulate(List<string> instructions, out int acc)
        {
            int[] instructionExecutionCount = new int[instructions.Count];
            acc = 0;
            int pc = 0;
            while (pc < instructions.Count && instructionExecutionCount[pc] == 0)
            {
                instructionExecutionCount[pc]++;

                string instruction = instructions[pc];
                string[] instructionParts = instruction.Split(' ');
                string opcode = instructionParts[0];
                int numericArg = int.Parse(instructionParts[1]);

                switch (opcode)
                {
                    case "nop":
                        pc += 1;
                        break;
                    case "acc":
                        pc += 1;
                        acc += numericArg;
                        break;
                    case "jmp":
                        pc += numericArg;
                        break;
                }
            }

            return pc < instructions.Count;
        }
    }
}
