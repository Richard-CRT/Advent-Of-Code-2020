using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_3___Toboggan_Trajectory
{
    public enum GridType { Open, Tree };

    public class Program
    {

        public static GridType[][] map;

        public static int width;
        public static int height;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            width = inputList[0].Length;
            height = inputList.Count;

            map = new GridType[height][];
            for (int height_i = 0; height_i < height; height_i++)
            {
                map[height_i] = new GridType[width];
                for (int width_i = 0; width_i < width; width_i++)
                {
                    if (inputList[height_i][width_i] == '.')
                        map[height_i][width_i] = GridType.Open;
                    else
                        map[height_i][width_i] = GridType.Tree;
                }
            }

            int trees_1_1 = CountTrees(1, 1);
            int trees_1_3 = CountTrees(1, 3); 
            int trees_1_5 = CountTrees(1, 5); 
            int trees_1_7 = CountTrees(1, 7); 
            int trees_2_1 = CountTrees(2, 1); 
            Console.WriteLine(trees_1_3);
            Console.WriteLine(trees_1_1 * trees_1_3 * trees_1_5 * trees_1_7 * trees_2_1);
            Console.ReadLine();
        }

        static int CountTrees(int yInc, int xInc)
        {
            int positionY = 0;
            int positionX = 0;

            int trees = 0;
            while (positionY < height)
            {
                int clampedPositionX = positionX % width;

                if (map[positionY][clampedPositionX] == GridType.Tree)
                    trees++;

                positionX += xInc;
                positionY += yInc;
            }

            return trees;
        }
    }
}
