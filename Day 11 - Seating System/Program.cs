using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_11___Seating_System
{
    enum GridCell { Empty, AvailableChair, OccupiedChair };

    class Program
    {
        static GridCell[][] grid;
        static int height;
        static int width;

        static GridCell FindFirstChair(int x, int y, int xDiff, int yDiff)
        {
            int multiple = 1;
            while (true)
            {
                int trialX = x + (multiple * xDiff);
                int trialY = y + (multiple * yDiff);
                if (trialX >= 0 && trialX <= width - 1
                    && trialY >= 0 && trialY <= height - 1)
                {
                    if (grid[trialY][trialX] == GridCell.AvailableChair
                        || grid[trialY][trialX] == GridCell.OccupiedChair)
                    {
                        return grid[trialY][trialX];
                    }
                }
                else
                {
                    return GridCell.Empty;
                }
                multiple++;
            }
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            height = inputList.Count;
            width = inputList[0].Length;
            grid = new GridCell[height][];
            for (int y = 0; y < height; y++)
            {
                grid[y] = new GridCell[width];
                for (int x = 0; x < width; x++)
                {
                    switch (inputList[y][x])
                    {
                        case 'L':
                            grid[y][x] = GridCell.AvailableChair;
                            break;
                        case '.':
                            grid[y][x] = GridCell.Empty;
                            break;
                        default:
                            throw new ArgumentException("Expecting only L and .");
                    }
                }
            }

            bool changeMade = true;
            while (changeMade)
            {
                changeMade = false;
                GridCell[][] copyOfGrid = new GridCell[height][];
                for (int y = 0; y < height; y++)
                {
                    copyOfGrid[y] = new GridCell[width];
                    for (int x = 0; x < width; x++)
                    {
                        copyOfGrid[y][x] = grid[y][x];
                    }
                }


                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid[y][x] != GridCell.Empty)
                        {
                            int occupiedAdjacentSeats = 0;
                            // left
                            if (x > 0 && grid[y][x - 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // right
                            if (x < width - 1 && grid[y][x + 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // top
                            if (y > 0 && grid[y - 1][x] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // bottom
                            if (y < height - 1 && grid[y + 1][x] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // tl
                            if (x > 0 && y > 0 && grid[y - 1][x - 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // bl
                            if (x > 0 && y < height - 1 && grid[y + 1][x - 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // tr
                            if (x < width - 1 && y > 0 && grid[y - 1][x + 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // br
                            if (x < width - 1 && y < height - 1 && grid[y + 1][x + 1] == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;

                            if (grid[y][x] == GridCell.AvailableChair)
                            {
                                if (occupiedAdjacentSeats == 0)
                                {
                                    changeMade = true;
                                    copyOfGrid[y][x] = GridCell.OccupiedChair;
                                }
                            }
                            else if (grid[y][x] == GridCell.OccupiedChair)
                            {
                                if (occupiedAdjacentSeats >= 4)
                                {
                                    changeMade = true;
                                    copyOfGrid[y][x] = GridCell.AvailableChair;
                                }
                            }
                        }
                    }
                }

                grid = copyOfGrid;
            }

            int occupiedSeatCount = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[y][x] == GridCell.OccupiedChair)
                    {
                        occupiedSeatCount++;
                    }
                }
            }
            Console.WriteLine(occupiedSeatCount);


            //////////////////////////////////////
            // PART 2
            //////////////////////////////////////

            grid = new GridCell[height][];
            for (int y = 0; y < height; y++)
            {
                grid[y] = new GridCell[width];
                for (int x = 0; x < width; x++)
                {
                    switch (inputList[y][x])
                    {
                        case 'L':
                            grid[y][x] = GridCell.AvailableChair;
                            break;
                        case '.':
                            grid[y][x] = GridCell.Empty;
                            break;
                        default:
                            throw new ArgumentException("Expecting only L and .");
                    }
                }
            }

            changeMade = true;
            while (changeMade)
            {
                changeMade = false;
                GridCell[][] copyOfGrid = new GridCell[height][];
                for (int y = 0; y < height; y++)
                {
                    copyOfGrid[y] = new GridCell[width];
                    for (int x = 0; x < width; x++)
                    {
                        copyOfGrid[y][x] = grid[y][x];
                    }
                }
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid[y][x] != GridCell.Empty)
                        {
                            int occupiedAdjacentSeats = 0;
                            // left
                            if (FindFirstChair(x, y, -1, 0) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // right
                            if (FindFirstChair(x, y, +1, 0) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // top
                            if (FindFirstChair(x, y, 0, -1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // bottom
                            if (FindFirstChair(x, y, 0, +1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // tl
                            if (FindFirstChair(x, y, -1, -1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // bl
                            if (FindFirstChair(x, y, -1, +1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // tr
                            if (FindFirstChair(x, y, +1, -1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;
                            // br
                            if (FindFirstChair(x, y, +1, +1) == GridCell.OccupiedChair)
                                occupiedAdjacentSeats++;

                            if (grid[y][x] == GridCell.AvailableChair)
                            {
                                if (occupiedAdjacentSeats == 0)
                                {
                                    changeMade = true;
                                    copyOfGrid[y][x] = GridCell.OccupiedChair;
                                }
                            }
                            else if (grid[y][x] == GridCell.OccupiedChair)
                            {
                                if (occupiedAdjacentSeats >= 5)
                                {
                                    changeMade = true;
                                    copyOfGrid[y][x] = GridCell.AvailableChair;
                                }
                            }
                        }
                    }
                }

                grid = copyOfGrid;
            }

            occupiedSeatCount = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[y][x] == GridCell.OccupiedChair)
                    {
                        occupiedSeatCount++;
                    }
                }
            }
            Console.WriteLine(occupiedSeatCount);

            Console.ReadLine();
        }
    }
}
