using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_24___Lobby_Layout
{
    class Program
    {
        public static Dictionary<int, Dictionary<int, Tile>> TilesNEE = new Dictionary<int, Dictionary<int, Tile>>();

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            Tile referenceTile = Tile.Create(0, 0);

            /////////////////////////////////////
            // Part 1
            /////////////////////////////////////

            foreach (string tileIdentifierString in inputList)
            {
                Tile currentTile = referenceTile;
                for (int char_i = 0; char_i < tileIdentifierString.Length; char_i++)
                {
                    string direction = "";
                    direction += tileIdentifierString[char_i];
                    if (direction == "s" || direction == "n")
                    {
                        direction += tileIdentifierString[char_i + 1];
                        char_i++;
                    }

                    switch (direction)
                    {
                        case "e":
                            currentTile = currentTile.e;
                            break;
                        case "w":
                            currentTile = currentTile.w;
                            break;
                        case "se":
                            currentTile = currentTile.se;
                            break;
                        case "sw":
                            currentTile = currentTile.sw;
                            break;
                        case "ne":
                            currentTile = currentTile.ne;
                            break;
                        case "nw":
                            currentTile = currentTile.nw;
                            break;
                    }
                }
                if (!currentTile.white)
                {

                }
                currentTile.white = !currentTile.white;
            }

            int blackTileCount = 0;
            foreach (var temp in TilesNEE.Values)
            {
                foreach (var tile in temp.Values)
                {
                    if (!tile.white)
                    {
                        blackTileCount++;
                    }
                }
            }

            Console.WriteLine(blackTileCount);

            /////////////////////////////////////
            // Part 2
            /////////////////////////////////////

            for (int day = 0; day < 100; day++)
            {
                List<Tile> tilesToGenerateBorderFor = new List<Tile>();
                foreach (var temp in TilesNEE.Values)
                {
                    foreach (var tile in temp.Values)
                    {
                        tilesToGenerateBorderFor.Add(tile);
                    }
                }
                foreach (Tile tile in tilesToGenerateBorderFor)
                {
                    Tile t;
                    t = tile.e;
                    t = tile.w;
                    t = tile.ne;
                    t = tile.sw;
                    t = tile.nw;
                    t = tile.se;
                }

                List<Tile> tilesToCheckForFlip = new List<Tile>();
                foreach (var temp in TilesNEE.Values)
                {
                    foreach (var tile in temp.Values)
                    {
                        tilesToCheckForFlip.Add(tile);
                    }
                }
                List<Tile> tilesToFlip = new List<Tile>();
                foreach (Tile tile in tilesToCheckForFlip)
                {
                        int adjacentBlackTiles = 0;
                        if (!tile.e.white) adjacentBlackTiles++;
                        if (!tile.w.white) adjacentBlackTiles++;
                        if (!tile.ne.white) adjacentBlackTiles++;
                        if (!tile.sw.white) adjacentBlackTiles++;
                        if (!tile.nw.white) adjacentBlackTiles++;
                        if (!tile.se.white) adjacentBlackTiles++;
                        if (tile.white && adjacentBlackTiles == 2)
                            tilesToFlip.Add(tile);
                        else if (!tile.white && (adjacentBlackTiles == 0 || adjacentBlackTiles > 2))
                            tilesToFlip.Add(tile);
                }
                foreach (Tile tile in tilesToFlip)
                    tile.white = !tile.white;
            }

            blackTileCount = 0;
            foreach (var temp in TilesNEE.Values)
            {
                foreach (var tile in temp.Values)
                {
                    if (!tile.white)
                        blackTileCount++;
                }
            }
            Console.WriteLine(blackTileCount);
            Console.ReadLine();
        }

        public class HexagonCoord
        {
            public int e;
            public int ne;

            public HexagonCoord(int e, int ne)
            {
                this.e = e;
                this.ne = ne;
            }
        }

        public class Tile
        {
            private Tile _e = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile e
            {
                get
                {
                    if (_e == null)
                        _e = Tile.Create(this.coord.e + 1, this.coord.ne);
                    return _e;
                }
                set { _e = value; }
            }

            private Tile _w = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile w
            {
                get
                {
                    if (_w == null)
                        _w = Tile.Create(this.coord.e - 1, this.coord.ne);
                    return _w;
                }
                set { _w = value; }
            }

            private Tile _ne = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile ne
            {
                get
                {
                    if (_ne == null)
                        _ne = Tile.Create(this.coord.e, this.coord.ne + 1);
                    return _ne;
                }
                set { _ne = value; }
            }

            private Tile _nw = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile nw
            {
                get
                {
                    if (_nw == null)
                        _nw = Tile.Create(this.coord.e - 1, this.coord.ne + 1);
                    return _nw;
                }
                set { _nw = value; }
            }

            private Tile _se = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile se
            {
                get
                {
                    if (_se == null)
                        _se = Tile.Create(this.coord.e + 1, this.coord.ne - 1);
                    return _se;
                }
                set { _se = value; }
            }

            private Tile _sw = null;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public Tile sw
            {
                get
                {
                    if (_sw == null)
                        _sw = Tile.Create(this.coord.e, this.coord.ne - 1);
                    return _sw;
                }
                set { _sw = value; }
            }


            public bool white = true;

            public HexagonCoord coord;

            public Tile(int e, int ne)
            {
                this.coord = new HexagonCoord(e, ne);
            }

            public override string ToString()
            {
                return $"{coord.e},{coord.ne}";
            }

            public static Tile Create(int e, int ne)
            {
                if (!TilesNEE.ContainsKey(ne))
                    TilesNEE[ne] = new Dictionary<int, Tile>();
                Tile newTile = new Tile(e, ne);
                TilesNEE[ne][e] = newTile;

                if (TilesNEE.ContainsKey(ne) && TilesNEE[ne].ContainsKey(e + 1))
                {
                    Tile tempTile = TilesNEE[ne][e + 1];
                    newTile.e = tempTile;
                    tempTile.w = newTile;
                }

                if (TilesNEE.ContainsKey(ne) && TilesNEE[ne].ContainsKey(e - 1))
                {
                    Tile tempTile = TilesNEE[ne][e - 1];
                    newTile.w = tempTile;
                    tempTile.e = newTile;
                }

                if (TilesNEE.ContainsKey(ne + 1) && TilesNEE[ne + 1].ContainsKey(e))
                {
                    Tile tempTile = TilesNEE[ne + 1][e];
                    newTile.ne = tempTile;
                    tempTile.sw = newTile;
                }

                if (TilesNEE.ContainsKey(ne - 1) && TilesNEE[ne - 1].ContainsKey(e))
                {
                    Tile tempTile = TilesNEE[ne - 1][e];
                    newTile.sw = tempTile;
                    tempTile.ne = newTile;
                }

                if (TilesNEE.ContainsKey(ne + 1) && TilesNEE[ne + 1].ContainsKey(e - 1))
                {
                    Tile tempTile = TilesNEE[ne + 1][e - 1];
                    newTile.nw = tempTile;
                    tempTile.se = newTile;
                }

                if (TilesNEE.ContainsKey(ne - 1) && TilesNEE[ne - 1].ContainsKey(e + 1))
                {
                    Tile tempTile = TilesNEE[ne - 1][e + 1];
                    newTile.se = tempTile;
                    tempTile.nw = newTile;
                }

                return newTile;
            }
        }
    }
}