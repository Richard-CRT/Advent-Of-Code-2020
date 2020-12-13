using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_5___Binary_Boarding
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            List<Seat> seats = inputList.ConvertAll(str => new Seat(str));
            
            seats = seats.OrderBy(seat => seat.id).ToList();

            int lastSeatId = 0;
            foreach (Seat seat in seats)
            {
                if (lastSeatId != 0 && seat.id != lastSeatId + 1)
                {
                    break;
                }
                else
                {
                    lastSeatId = seat.id;
                }
            }

            Console.WriteLine(seats.Last().id);
            Console.WriteLine(lastSeatId + 1);
            Console.ReadLine();
        }
    }

    class Seat
    {
        int row;
        int column;
        public int id;

        public Seat(string identification_string)
        {
            int rowTop = 127;
            int rowBottom = 0;
            int columnTop = 7;
            int columnBottom = 0;

            foreach (char character in identification_string)
            {
                if (character == 'F')
                {
                    rowTop = rowBottom + (int)Math.Floor((rowTop - rowBottom) / (float)2);
                }
                else if (character == 'B')
                {
                    rowBottom = rowBottom + (int)Math.Ceiling((rowTop - rowBottom) / (float)2);
                }
                else if (character == 'L')
                {
                    columnTop = columnBottom + (int)Math.Floor((columnTop - columnBottom) / (float)2);
                }
                else if (character == 'R')
                {
                    columnBottom = columnBottom + (int)Math.Ceiling((columnTop - columnBottom) / (float)2);
                }
                else
                {
                    throw new ArgumentException("Expecting F, B, L, or R");
                }
            }

            if (rowTop != rowBottom || columnTop != columnBottom)
            {
                throw new ArgumentException("Top and Bottom should be narrowed to the same by now");
            }

            this.row = rowBottom;
            this.column = columnBottom;
            this.id = (this.row * 8) + this.column;
        }
    }
}
