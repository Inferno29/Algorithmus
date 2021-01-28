using System;
using System.Collections.Generic;
using System.Text;

namespace pMedian
{
    //Class only contains method
    public static class ObjectiveFunction
    {


        public static double ObjectiveValue(FileHandler fh, Chromosome bin)
        {
            double objValue = 0;

            for (int i = 0; i < fh.cost.Length; i++)
            {
                for (int j = 0; j < fh.cost.Length; j++)
                {
                    objValue += fh.cost[i, j] * bin.solution[i, j];
                }
            }



            return objValue;
        }

    }
}
