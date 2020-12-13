using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_12___Rain_Risk
{
    class Program
    {
        static void Main(string[] args)
        {
            int positionX = 0;
            int positionY = 0;
            int angleFromNorth = 90;

            List<string> inputList = AoCUtilities.GetInputLines();
            foreach (string instruction in inputList)
            {
                int numericArg = int.Parse(instruction.Substring(1));
                switch (instruction[0])
                {
                    case 'N':
                        positionY += numericArg;
                        break;
                    case 'S':
                        positionY -= numericArg;
                        break;
                    case 'E':
                        positionX += numericArg;
                        break;
                    case 'W':
                        positionX -= numericArg;
                        break;
                    case 'R':
                        angleFromNorth += numericArg;
                        angleFromNorth = angleFromNorth % 360;
                        if (angleFromNorth < 0)
                            angleFromNorth += 360;
                        break;
                    case 'L':
                        angleFromNorth -= numericArg;
                        angleFromNorth = angleFromNorth % 360;
                        if (angleFromNorth < 0)
                            angleFromNorth += 360;
                        break;
                    case 'F':
                        positionX += numericArg * (int)Math.Round(Math.Sin(angleFromNorth * (Math.PI / 180)));
                        positionY += numericArg * (int)Math.Round(Math.Cos(angleFromNorth * (Math.PI / 180)));
                        break;
                }
            }

            Console.WriteLine(Math.Abs(positionX) + Math.Abs(positionY));

            /////////////////////////////////////////
            // Part 2
            /////////////////////////////////////////

            positionX = 0;
            positionY = 0;
            int waypointOffsetX = 10;
            int waypointOffsetY = 1;

            foreach (string instruction in inputList)
            {
                int numericArg = int.Parse(instruction.Substring(1));
                switch (instruction[0])
                {
                    case 'N':
                        waypointOffsetY += numericArg;
                        break;
                    case 'S':
                        waypointOffsetY -= numericArg;
                        break;
                    case 'E':
                        waypointOffsetX += numericArg;
                        break;
                    case 'W':
                        waypointOffsetX -= numericArg;
                        break;
                    case 'R':
                        {
                            int temp;
                            if (numericArg == 90)
                            {
                                temp = waypointOffsetY;
                                waypointOffsetY = -waypointOffsetX;
                                waypointOffsetX = temp;
                            }
                            else if (numericArg == 180)
                            {
                                waypointOffsetY = -waypointOffsetY;
                                waypointOffsetX = -waypointOffsetX;
                            }
                            else if (numericArg == 270)
                            {
                                temp = waypointOffsetY;
                                waypointOffsetY = waypointOffsetX;
                                waypointOffsetX = -temp;
                            }
                        }
                        break;
                    case 'L':
                        {
                            int temp;
                            if (numericArg == 270)
                            {
                                temp = waypointOffsetY;
                                waypointOffsetY = -waypointOffsetX;
                                waypointOffsetX = temp;
                            }
                            else if (numericArg == 180)
                            {
                                waypointOffsetY = -waypointOffsetY;
                                waypointOffsetX = -waypointOffsetX;
                            }
                            else if (numericArg == 90)
                            {
                                temp = waypointOffsetY;
                                waypointOffsetY = waypointOffsetX;
                                waypointOffsetX = -temp;
                            }
                        }
                        break;
                    case 'F':
                        positionX += numericArg * waypointOffsetX;
                        positionY += numericArg * waypointOffsetY;
                        break;
                }
            }

            Console.WriteLine(Math.Abs(positionX) + Math.Abs(positionY));
            Console.ReadLine();
        }
    }
}
