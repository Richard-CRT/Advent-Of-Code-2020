using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_17___Conway_Cubes
{
    class Program
    {
        static int minX = int.MaxValue;
        static int maxX = int.MinValue;
        static int minY = int.MaxValue;
        static int maxY = int.MinValue;
        static int minZ = int.MaxValue;
        static int maxZ = int.MinValue;
        static int minW = int.MaxValue;
        static int maxW = int.MinValue;
        static Dictionary<int, Dictionary<int, Dictionary<int, Cube>>> threeDimWorld = new Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>();
        static Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>> fourDimWorld = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>>();

        static List<Cube> getNeighbourCubes(int x, int y, int z)
        {
            List<Cube> neighbourCubes = new List<Cube>();
            for (int _z = -1; _z <= 1; _z++)
            {
                for (int _y = -1; _y <= 1; _y++)
                {
                    for (int _x = -1; _x <= 1; _x++)
                    {
                        if (_z != 0 || _y != 0 || _x != 0)
                            neighbourCubes.Add(getCube(x + _x, y + _y, z + _z));
                    }
                }
            }
            return neighbourCubes;
        }

        static void setCube(int x, int y, int z, Cube cube)
        {
            if (!threeDimWorld.ContainsKey(z))
            {
                threeDimWorld[z] = new Dictionary<int, Dictionary<int, Cube>>();
            }
            if (!threeDimWorld[z].ContainsKey(y))
            {
                threeDimWorld[z][y] = new Dictionary<int, Cube>();
            }

            if (x < minX)
                minX = x;
            if (x > maxX)
                maxX = x;
            if (y < minY)
                minY = y;
            if (y > maxY)
                maxY = y;
            if (z < minZ)
                minZ = z;
            if (z > maxZ)
                maxZ = z;
            threeDimWorld[z][y][x] = cube;
        }

        static Cube getCube(int x, int y, int z)
        {
            if (!threeDimWorld.ContainsKey(z))
            {
                threeDimWorld[z] = new Dictionary<int, Dictionary<int, Cube>>();
            }
            if (!threeDimWorld[z].ContainsKey(y))
            {
                threeDimWorld[z][y] = new Dictionary<int, Cube>();
            }
            if (!threeDimWorld[z][y].ContainsKey(x))
            {
                Cube newCube = new Cube(x, y, z, false);
                setCube(x, y, z, newCube);
            }

            return threeDimWorld[z][y][x];
        }

        static List<Cube> getNeighbourCubes(int x, int y, int z, int w)
        {
            List<Cube> neighbourCubes = new List<Cube>();
            for (int _w = -1; _w <= 1; _w++)
            {
                for (int _z = -1; _z <= 1; _z++)
                {
                    for (int _y = -1; _y <= 1; _y++)
                    {
                        for (int _x = -1; _x <= 1; _x++)
                        {
                            if (_w != 0 || _z != 0 || _y != 0 || _x != 0)
                                neighbourCubes.Add(getCube(x + _x, y + _y, z + _z, w + _w));
                        }
                    }
                }
            }
            return neighbourCubes;
        }

        static void setCube(int x, int y, int z, int w, Cube cube)
        {
            if (!fourDimWorld.ContainsKey(w))
            {
                fourDimWorld[w] = new Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>();
            }
            if (!fourDimWorld[w].ContainsKey(z))
            {
                fourDimWorld[w][z] = new Dictionary<int, Dictionary<int, Cube>>();
            }
            if (!fourDimWorld[w][z].ContainsKey(y))
            {
                fourDimWorld[w][z][y] = new Dictionary<int, Cube>();
            }

            if (x < minX)
                minX = x;
            if (x > maxX)
                maxX = x;
            if (y < minY)
                minY = y;
            if (y > maxY)
                maxY = y;
            if (z < minZ)
                minZ = z;
            if (z > maxZ)
                maxZ = z;
            if (w < minW)
                minW = w;
            if (w > maxW)
                maxW = w;
            fourDimWorld[w][z][y][x] = cube;
        }

        static Cube getCube(int x, int y, int z, int w)
        {
            if (!fourDimWorld.ContainsKey(w))
            {
                fourDimWorld[w] = new Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>();
            }
            if (!fourDimWorld[w].ContainsKey(z))
            {
                fourDimWorld[w][z] = new Dictionary<int, Dictionary<int, Cube>>();
            }
            if (!fourDimWorld[w][z].ContainsKey(y))
            {
                fourDimWorld[w][z][y] = new Dictionary<int, Cube>();
            }
            if (!fourDimWorld[w][z][y].ContainsKey(x))
            {
                Cube newCube = new Cube(x, y, z, w, false);
                setCube(x, y, z, w, newCube);
            }

            return fourDimWorld[w][z][y][x];
        }

        static void print()
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                Console.WriteLine($"z = {z}");
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Cube cube = getCube(x, y, z);
                        if (cube.active)
                            Console.Write("#");
                        else
                            Console.Write(".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        static void print4d()
        {
            for (int w = minW; w <= maxW; w++)
            {
                Console.WriteLine($"w = {w}");
                for (int z = minZ; z <= maxZ; z++)
                {
                    Console.WriteLine($"z = {z}");
                    for (int y = minY; y <= maxY; y++)
                    {
                        for (int x = minX; x <= maxX; x++)
                        {
                            Cube cube = getCube(x, y, z);
                            if (cube.active)
                                Console.Write("#");
                            else
                                Console.Write(".");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            /*
             *   y
             *   ^
             *   |
             * z x---> x
             * 
             * 
             */
            // Planes : Rows : Cube

            ///////////////////////////////////////
            // Part 1
            ///////////////////////////////////////
            
            for (int y = 0; y < inputList.Count; y++)
            {
                string rowString = inputList[y];
                for (int x = 0; x < rowString.Length; x++)
                {
                    char cubeChar = rowString[x];

                    Coord coord = new Coord(x, y, 0);
                    switch (cubeChar)
                    {
                        case '.':
                            setCube(x, y, 0, new Cube(x, y, 0, false));
                            break;
                        case '#':
                            setCube(x, y, 0, new Cube(x, y, 0, true));
                            break;
                    }
                }
            }

            //print();

            for (int cycle = 0; cycle < 6; cycle++)
            {
                int _minZ = minZ;
                int _maxZ = maxZ;
                int _minY = minY;
                int _maxY = maxY;
                int _minX = minX;
                int _maxX = maxX;
                for (int z = _minZ - 1; z <= _maxZ + 1; z++)
                {
                    for (int y = _minY - 1; y <= _maxY + 1; y++)
                    {
                        for (int x = _minX - 1; x <= _maxX + 1; x++)
                        {
                            Cube cube = getCube(x, y, z);
                            List<Cube> neighbourCubes = getNeighbourCubes(x, y, z);
                            int howManyActiveNeighbours = neighbourCubes.Count(c => c.active);

                            if (cube.active)
                                cube.newActive = howManyActiveNeighbours == 2 || howManyActiveNeighbours == 3;
                            else
                                cube.newActive = howManyActiveNeighbours == 3;
                        }
                    }
                }

                foreach (var twoDimWorldKV in threeDimWorld)
                {
                    foreach (var oneDimWorldKV in twoDimWorldKV.Value)
                    {
                        foreach (var cubeKV in oneDimWorldKV.Value)
                        {
                            cubeKV.Value.active = cubeKV.Value.newActive;
                        }
                    }
                }

                //Console.WriteLine($"Cycle {cycle + 1}");
                //print();
            }

            int activeCountP1 = 0;
            foreach (var twoDimWorldKV in threeDimWorld)
            {
                foreach (var oneDimWorldKV in twoDimWorldKV.Value)
                {
                    foreach (var cubeKV in oneDimWorldKV.Value)
                    {
                        if (cubeKV.Value.active)
                            activeCountP1++;
                    }
                }
            }
            Console.WriteLine(activeCountP1);

            ///////////////////////////////////////
            // Part 2
            ///////////////////////////////////////

            minX = int.MaxValue;
            maxX = int.MinValue;
            minY = int.MaxValue;
            maxY = int.MinValue;
            minZ = int.MaxValue;
            maxZ = int.MinValue;
            minW = int.MaxValue;
            maxW = int.MinValue;

            for (int y = 0; y < inputList.Count; y++)
            {
                string rowString = inputList[y];
                for (int x = 0; x < rowString.Length; x++)
                {
                    char cubeChar = rowString[x];

                    Coord coord = new Coord(x, y, 0);
                    switch (cubeChar)
                    {
                        case '.':
                            setCube(x, y, 0, 0, new Cube(x, y, 0, 0, false));
                            break;
                        case '#':
                            setCube(x, y, 0, 0, new Cube(x, y, 0, 0, true));
                            break;
                    }
                }
            }

            //print4d();

            for (int cycle = 0; cycle < 6; cycle++)
            {
                int _minW = minW;
                int _maxW = maxW;
                int _minZ = minZ;
                int _maxZ = maxZ;
                int _minY = minY;
                int _maxY = maxY;
                int _minX = minX;
                int _maxX = maxX;
                for (int w = _minW - 1; w <= _maxW + 1; w++)
                {
                    for (int z = _minZ - 1; z <= _maxZ + 1; z++)
                    {
                        for (int y = _minY - 1; y <= _maxY + 1; y++)
                        {
                            for (int x = _minX - 1; x <= _maxX + 1; x++)
                            {
                                Cube cube = getCube(x, y, z, w);
                                List<Cube> neighbourCubes = getNeighbourCubes(x, y, z, w);
                                int howManyActiveNeighbours = neighbourCubes.Count(c => c.active);

                                if (cube.active)
                                    cube.newActive = howManyActiveNeighbours == 2 || howManyActiveNeighbours == 3;
                                else
                                    cube.newActive = howManyActiveNeighbours == 3;
                            }
                        }
                    }
                }

                foreach (var threeDimWorldKV in fourDimWorld)
                {
                    foreach (var twoDimWorldKV in threeDimWorldKV.Value)
                    {
                        foreach (var oneDimWorldKV in twoDimWorldKV.Value)
                        {
                            foreach (var cubeKV in oneDimWorldKV.Value)
                            {
                                cubeKV.Value.active = cubeKV.Value.newActive;
                            }
                        }
                    }
                }

                //Console.WriteLine($"Cycle {cycle + 1}");
                //print4d();
            }

            int activeCountP2 = 0;
            foreach (var threeDimWorldKV in fourDimWorld)
            {
                foreach (var twoDimWorldKV in threeDimWorldKV.Value)
                {
                    foreach (var oneDimWorldKV in twoDimWorldKV.Value)
                    {
                        foreach (var cubeKV in oneDimWorldKV.Value)
                        {
                            if (cubeKV.Value.active)
                                activeCountP2++;
                        }
                    }
                }
            }
            Console.WriteLine(activeCountP2);

            Console.ReadLine();
        }
    }

    class Cube
    {
        public bool active = false;
        public bool newActive = false;

        public Coord coord;

        public Cube(int x, int y, int z, bool active) : this(x, y, z, 0, active)
        {

        }

        public Cube(int x, int y, int z, int w, bool active)
        {
            this.coord = new Coord(x, y, z, w);
            this.active = active;
        }

        public override string ToString()
        {
            if (active)
                return $"Active {coord.ToString()}";
            else
                return $"Inactive {coord.ToString()}";
        }
    }

    class Coord
    {
        public int x;
        public int y;
        public int z;
        public int w;

        public Coord(int x, int y, int z, int w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public override string ToString()
        {
            return $"({x},{y},{z},{w})";
        }
    }
}
