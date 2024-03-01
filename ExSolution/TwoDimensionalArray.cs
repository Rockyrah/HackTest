using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExSolution
{
    public class TwoDimensionalArray
    {
        public static int[,] arraytwo = new int[2, 2];

        public static int[,] DisplayTwoDimensional()
        {
            for (int i = 0; i < 2; i++)
            {
                for(int j=1; j<2; j++)
                {
                    arraytwo[i, 0] = j; 
                    arraytwo[i,j] = 11121;
                }
            }

            return arraytwo;
        }

        public static List<List<object>> GetTwoDimensionalMixed(int rows, int cols)
        {
            List<List<object>> result = new List<List<object>>();

            for(int i=0; i< rows; i++)
            {
                List<object> row = new List<object>();

                for(int j=0; j< cols; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        row.Add(i * cols + j + 1);
                    }
                    else
                    {
                        row.Add((i * cols + j + 1) * 0.5M);
                    }
                }
                result.Add(row);
            }
            return result;
        }

        
    }
}
