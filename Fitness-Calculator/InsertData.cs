using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Calculator
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines()]
    [IgnoreFirst()]
    class InsertData
    {
        public string Keyword;
        public int Page;
        public int X;
        public int Y;
        public string Font;
        public string Color;
        public int Size;
        public string Allignment;

        public void Print()
        {
            Console.WriteLine(Keyword);
        }
    }
}
