using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_20___Jurassic_Jigsaw
{
    class Program
    {

        static int searchForSeaMonsters(Image image)
        {
            int seaMonsters = 0;
            for (int y = 0; y < image.dim; y++)
            {
                for (int x = 0; x < image.dim; x++)
                {
                    bool seaMonster =
                            x + 19 < image.dim &&
                            y + 2 < image.dim &&
                        image.pixels[y][x + 18].type == PixelType.white &&
                        image.pixels[y + 1][x].type == PixelType.white &&
                        image.pixels[y + 1][x + 5].type == PixelType.white &&
                        image.pixels[y + 1][x + 11].type == PixelType.white &&
                        image.pixels[y + 1][x + 12].type == PixelType.white &&
                        image.pixels[y + 1][x + 17].type == PixelType.white &&
                        image.pixels[y + 1][x + 18].type == PixelType.white &&
                        image.pixels[y + 1][x + 19].type == PixelType.white &&
                        image.pixels[y + 2][x + 1].type == PixelType.white &&
                        image.pixels[y + 2][x + 4].type == PixelType.white &&
                        image.pixels[y + 2][x + 7].type == PixelType.white &&
                        image.pixels[y + 2][x + 10].type == PixelType.white &&
                        image.pixels[y + 2][x + 13].type == PixelType.white &&
                        image.pixels[y + 2][x + 16].type == PixelType.white;
                    if (seaMonster)
                        seaMonsters++;
                }
            }
            return seaMonsters;
        }

        static void Main(string[] args)
        {
            string input = AoCUtilities.GetInput();

            /////////////////////
            // Part 1
            /////////////////////
            
            List<Tile> tilesR0 = new List<Tile>();
            List<Tile> tilesR90 = new List<Tile>();
            List<Tile> tilesR180 = new List<Tile>();
            List<Tile> tilesR270 = new List<Tile>();
            List<Tile> flippedTilesR0 = new List<Tile>();
            List<Tile> flippedTilesR90 = new List<Tile>();
            List<Tile> flippedTilesR180 = new List<Tile>();
            List<Tile> flippedTilesR270 = new List<Tile>();

            var tileStrings = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tileString in tileStrings)
            {
                var tileLines = tileString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string idString = tileLines[0].Substring(5, tileLines[0].Length - 6);
                int id = int.Parse(idString);

                Tile newTileR0 = new Tile(id, tileLines);
                tilesR0.Add(newTileR0);
                Tile newTileR90 = newTileR0.GetR90Tile();
                tilesR90.Add(newTileR90);
                Tile newTileR180 = newTileR90.GetR90Tile();
                tilesR180.Add(newTileR180);
                Tile newTileR270 = newTileR180.GetR90Tile();
                tilesR270.Add(newTileR270);
                Tile flippedNewTileR0 = newTileR0.GetFlippedTile();
                flippedTilesR0.Add(flippedNewTileR0);
                Tile flippedNewTileR90 = flippedNewTileR0.GetR90Tile();
                flippedTilesR90.Add(flippedNewTileR90);
                Tile flippedNewTileR180 = flippedNewTileR90.GetR90Tile();
                flippedTilesR180.Add(flippedNewTileR180);
                Tile flippedNewTileR270 = flippedNewTileR180.GetR90Tile();
                flippedTilesR270.Add(flippedNewTileR270);
            }


            Tile anchorTile = tilesR0[0];
            List<Tile> usedTiles = new List<Tile> { anchorTile };

            anchorTile.FindMatch(usedTiles, tilesR0, tilesR90, tilesR180, tilesR270, flippedTilesR0, flippedTilesR90, flippedTilesR180, flippedTilesR270);

            long product = 1;
            Tile tlTile = null;
            foreach (Tile usedTile in usedTiles)
            {
                if (usedTile.TL)
                    tlTile = usedTile;
                if (usedTile.TL || usedTile.TR || usedTile.BL || usedTile.BR)
                {
                    product *= usedTile.id;
                }
            }
            Console.WriteLine(product);

            /////////////////////
            // Part 2
            /////////////////////

            int tilesDim = (int)Math.Sqrt(usedTiles.Count);
            Tile[][] tileMap = new Tile[tilesDim][];

            int tile_y = 0;
            Tile bottomTile = tlTile;
            while (bottomTile != null)
            {
                tileMap[tile_y] = new Tile[tilesDim];

                int tile_x = 0;
                Tile rightTile = bottomTile;
                while (rightTile != null)
                {
                    tileMap[tile_y][tile_x] = rightTile;
                    tile_x++;
                    rightTile = rightTile.rightTile;
                }
                tile_y++;
                bottomTile = bottomTile.bottomTile;
            }

            int imageDim = tilesDim * 8;
            Image imageR0 = new Image(imageDim);

            int whitePixelCount = 0;
            for (tile_y = 0; tile_y < tilesDim; tile_y++)
            {
                for (int tile_x = 0; tile_x < tilesDim; tile_x++)
                {
                    Tile tile = tileMap[tile_y][tile_x];
                    for (int pixel_y = 1; pixel_y <= 8; pixel_y++)
                    {
                        for (int pixel_x = 1; pixel_x <= 8; pixel_x++)
                        {
                            int globalPixel_y = (tile_y * 8) + (pixel_y - 1);
                            int globalPixel_x = (tile_x * 8) + (pixel_x - 1);
                            imageR0.pixels[globalPixel_y][globalPixel_x] = tile.pixels[pixel_y][pixel_x];
                            if (tile.pixels[pixel_y][pixel_x].type == PixelType.white)
                                whitePixelCount++;
                        }
                    }
                }
            }

            Image imageR90 = imageR0.GetR90Image();
            Image imageR180 = imageR90.GetR90Image();
            Image imageR270 = imageR180.GetR90Image();
            Image imageFlippedR0 = imageR0.GetFlippedImage();
            Image imageFlippedR90 = imageFlippedR0.GetR90Image();
            Image imageFlippedR180 = imageFlippedR90.GetR90Image();
            Image imageFlippedR270 = imageFlippedR180.GetR90Image();

            //imageR0.print();
            int seaMonsters = 0;

            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageR0);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageR90);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageR180);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageR270);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageFlippedR0);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageFlippedR90);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageFlippedR180);
            if (seaMonsters == 0)
                seaMonsters = searchForSeaMonsters(imageFlippedR270);

            int nonSeaMonstersWhitePixels = whitePixelCount - (seaMonsters * 15);

            Console.WriteLine(nonSeaMonstersWhitePixels);
            Console.ReadLine();
        }
    }

    class Image
    {
        public int dim;
        public Pixel[][] pixels;

        public Image(int imageDim)
        {
            pixels = new Pixel[imageDim][];
            this.dim = imageDim;
            for (int y = 0; y < imageDim; y++)
                pixels[y] = new Pixel[imageDim];
        }

        public Image GetFlippedImage()
        {
            Image flippedImage = new Image(this.dim);
            for (int y = 0; y < this.dim; y++)
            {
                for (int x = 0; x < this.dim; x++)
                {
                    flippedImage.pixels[y][x] = this.pixels[y][(this.dim - 1) - x];
                }
            }
            return flippedImage;
        }

        public Image GetR90Image()
        {
            Image r90Image = new Image(this.dim);
            for (int y = 0; y < this.dim; y++)
            {
                for (int x = 0; x < this.dim; x++)
                {
                    r90Image.pixels[y][x] = this.pixels[(this.dim - 1) - x][y];
                }
            }
            return r90Image;
        }

        public void print()
        {
#if DEBUG || OVERRIDE
            for (int y = 0; y < this.dim; y++)
            {
                for (int x = 0; x < this.dim; x++)
                {
                    AoCUtilities.DebugWrite("{0}", (char)this.pixels[y][x].type);
                }
                AoCUtilities.DebugWriteLine();
            }
            AoCUtilities.DebugWriteLine();
#endif
        }
    }

    class Tile
    {
        public int id;
        public Pixel[][] pixels = new Pixel[10][];

        public Tile leftTile = null;
        public Tile topTile = null;
        public Tile rightTile = null;
        public Tile bottomTile = null;

        public bool B { get { return topTile != null && bottomTile == null; } }
        public bool L { get { return rightTile != null && leftTile == null; } }
        public bool T { get { return bottomTile != null && topTile == null; } }
        public bool R { get { return leftTile != null && rightTile == null; } }
        public bool TL { get { return T && L; } }
        public bool TR { get { return T && R; } }
        public bool BL { get { return B && L; } }
        public bool BR { get { return B && R; } }


        public Tile(int id)
        {
            this.id = id;
            for (int i = 0; i < 10; i++)
                pixels[i] = new Pixel[10];
        }

        public Tile(int id, string[] tileLines)
        {
            this.id = id;
            for (int i = 0; i < 10; i++)
                pixels[i] = new Pixel[10];

            for (int line_i = 1; line_i < tileLines.Length; line_i++)
            {
                string line = tileLines[line_i];
                for (int char_i = 0; char_i < line.Length; char_i++)
                {
                    char character = line[char_i];
                    int x = char_i;
                    int y = line_i - 1;
                    Pixel newPixel = null;
                    switch (character)
                    {
                        case (char)PixelType.white:
                            newPixel = new Pixel(x, y, PixelType.white);
                            break;
                        case (char)PixelType.black:
                            newPixel = new Pixel(x, y, PixelType.black);
                            break;
                    }
                    this.pixels[y][x] = newPixel;
                }
            }
        }

        public Tile GetFlippedTile()
        {
            Tile flippedTile = new Tile(this.id);
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    flippedTile.pixels[y][x] = this.pixels[y][9 - x];
                }
            }
            return flippedTile;
        }

        public Tile GetR90Tile()
        {
            Tile r90Tile = new Tile(this.id);
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    r90Tile.pixels[y][x] = this.pixels[9 - x][y];
                }
            }
            return r90Tile;
        }

        public override string ToString()
        {
            return $"ID:{id}";
        }

        public Pixel[] GetTopEdge()
        {
            return pixels[0];
        }

        public Pixel[] GetBottomEdge()
        {
            return pixels[9];
        }

        public Pixel[] GetLeftEdge()
        {
            var column = new Pixel[10];
            for (int i = 0; i < 10; i++)
            {
                column[i] = pixels[i][0];
            }
            return column;
        }

        public Pixel[] GetRightEdge()
        {
            var column = new Pixel[10];
            for (int i = 0; i < 10; i++)
            {
                column[i] = pixels[i][9];
            }
            return column;
        }

        bool CompareEdge(Pixel[] edge1, Pixel[] edge2)
        {
            bool match = true;
            for (int i = 0; i < 10; i++)
            {
                if (edge1[i].type != edge2[i].type)
                {
                    match = false;
                    break;
                }
            }
            return match;
        }

        public void FindMatch(List<Tile> usedTiles, List<Tile> tilesR0, List<Tile> tilesR90, List<Tile> tilesR180, List<Tile> tilesR270,
                                List<Tile> flippedTilesR0, List<Tile> flippedTilesR90, List<Tile> flippedTilesR180, List<Tile> flippedTilesR270)
        {
            Pixel[] leftEdgeTile = this.GetLeftEdge();
            Pixel[] rightEdgeTile = this.GetRightEdge();
            Pixel[] topEdgeTile = this.GetTopEdge();
            Pixel[] bottomEdgeTile = this.GetBottomEdge();
            for (int otherTile_i = 0; otherTile_i < tilesR0.Count; otherTile_i++)
            {
                Tile otherTileR0 = tilesR0[otherTile_i];
                int otherTileId = otherTileR0.id;

                if (this.id != otherTileId)
                {
                    Tile otherTileR90 = tilesR90[otherTile_i];
                    Tile otherTileR180 = tilesR180[otherTile_i];
                    Tile otherTileR270 = tilesR270[otherTile_i];
                    Tile otherTileFlippedR0 = flippedTilesR0[otherTile_i];
                    Tile otherTileFlippedR90 = flippedTilesR90[otherTile_i];
                    Tile otherTileFlippedR180 = flippedTilesR180[otherTile_i];
                    Tile otherTileFlippedR270 = flippedTilesR270[otherTile_i];

                    bool match;
                    Pixel[] edgeTest;

                    match = false;
                    if (leftTile == null)
                    {
                        edgeTest = otherTileR0.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileR0;
                        edgeTest = otherTileR90.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileR90;
                        edgeTest = otherTileR180.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileR180;
                        edgeTest = otherTileR270.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileR270;
                        edgeTest = otherTileFlippedR0.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileFlippedR0;
                        edgeTest = otherTileFlippedR90.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileFlippedR90;
                        edgeTest = otherTileFlippedR180.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileFlippedR180;
                        edgeTest = otherTileFlippedR270.GetRightEdge();
                        if (!match && (match = CompareEdge(leftEdgeTile, edgeTest)))
                            leftTile = otherTileFlippedR270;
                    }
                    if (match)
                    {
                        leftTile.rightTile = this;
                        if (!usedTiles.Contains(leftTile.rightTile))
                            usedTiles.Add(leftTile.rightTile);
                        leftTile.FindMatch(usedTiles, tilesR0, tilesR90, tilesR180, tilesR270, flippedTilesR0, flippedTilesR90, flippedTilesR180, flippedTilesR270);
                    }

                    match = false;
                    if (rightTile == null)
                    {
                        edgeTest = otherTileR0.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileR0;
                        edgeTest = otherTileR90.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileR90;
                        edgeTest = otherTileR180.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileR180;
                        edgeTest = otherTileR270.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileR270;
                        edgeTest = otherTileFlippedR0.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileFlippedR0;
                        edgeTest = otherTileFlippedR90.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileFlippedR90;
                        edgeTest = otherTileFlippedR180.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileFlippedR180;
                        edgeTest = otherTileFlippedR270.GetLeftEdge();
                        if (!match && (match = CompareEdge(rightEdgeTile, edgeTest)))
                            rightTile = otherTileFlippedR270;
                    }
                    if (match)
                    {
                        rightTile.leftTile = this;
                        if (!usedTiles.Contains(rightTile.leftTile))
                            usedTiles.Add(rightTile.leftTile);
                        rightTile.FindMatch(usedTiles, tilesR0, tilesR90, tilesR180, tilesR270, flippedTilesR0, flippedTilesR90, flippedTilesR180, flippedTilesR270);
                    }

                    match = false;
                    if (bottomTile == null)
                    {
                        edgeTest = otherTileR0.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileR0;
                        edgeTest = otherTileR90.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileR90;
                        edgeTest = otherTileR180.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileR180;
                        edgeTest = otherTileR270.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileR270;
                        edgeTest = otherTileFlippedR0.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileFlippedR0;
                        edgeTest = otherTileFlippedR90.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileFlippedR90;
                        edgeTest = otherTileFlippedR180.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileFlippedR180;
                        edgeTest = otherTileFlippedR270.GetTopEdge();
                        if (!match && (match = CompareEdge(bottomEdgeTile, edgeTest)))
                            bottomTile = otherTileFlippedR270;
                    }
                    if (match)
                    {
                        bottomTile.topTile = this;
                        if (!usedTiles.Contains(bottomTile.topTile))
                            usedTiles.Add(bottomTile.topTile);
                        bottomTile.FindMatch(usedTiles, tilesR0, tilesR90, tilesR180, tilesR270, flippedTilesR0, flippedTilesR90, flippedTilesR180, flippedTilesR270);
                    }

                    match = false;
                    if (topTile == null)
                    {
                        edgeTest = otherTileR0.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileR0;
                        edgeTest = otherTileR90.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileR90;
                        edgeTest = otherTileR180.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileR180;
                        edgeTest = otherTileR270.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileR270;
                        edgeTest = otherTileFlippedR0.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileFlippedR0;
                        edgeTest = otherTileFlippedR90.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileFlippedR90;
                        edgeTest = otherTileFlippedR180.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileFlippedR180;
                        edgeTest = otherTileFlippedR270.GetBottomEdge();
                        if (!match && (match = CompareEdge(topEdgeTile, edgeTest)))
                            topTile = otherTileFlippedR270;
                    }
                    if (match)
                    {
                        topTile.bottomTile = this;
                        if (!usedTiles.Contains(topTile.bottomTile))
                            usedTiles.Add(topTile.bottomTile);
                        topTile.FindMatch(usedTiles, tilesR0, tilesR90, tilesR180, tilesR270, flippedTilesR0, flippedTilesR90, flippedTilesR180, flippedTilesR270);
                    }

                    // for each edge of tile, compare to all 8 edges of otherTile
                }
            }
        }
    }

    public enum PixelType { white = '#', black = '.' };
    class Pixel
    {
        public PixelType type;
        public int x;
        public int y;

        public Pixel(int x, int y, PixelType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public override string ToString()
        {
            return $"({x},{y}) {type}";
        }
    }
}
