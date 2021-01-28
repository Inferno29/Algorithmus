using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace pMedian
{
    public class LocalSearch
    {
        DataTable dt = new DataTable();
        public LocalSearch() 
        {

        }

        Chromosome bin = new Chromosome();
       


        public int[,] shuffleSolution(int[,] a)
        {

            
            int[,] b = bin.solution;


            int value = bin.chromosomeFitness;
            

            int n = a.GetLength(0);
            int m = a.GetLength(1);

            

            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    a[i,j] = swap(a, i + rand.Next(n - i), j + rand.Next(m - j), i, j);
                }
            }

            bin.solution = a;
            int value2 = bin.chromosomeFitness;

            return a;

        }


           
        public int swap(int[,] solution, int changeR, int changeC, int a, int b)
        {
            int temp = solution[a, b];
            solution[a, b] = solution[changeR, changeC];
            solution[changeR, changeC] = temp;

            return temp;
        }





        public DataTable FillDataTable(int[,] solution)
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < solution.GetLength(0); i++)
            {

                DataRow row = dt.NewRow();

                for (int j = 0; j < solution.GetLength(1); j++)
                {
                    row[j] = solution[i, j];
                }

                dt.Rows.Add(row);

            }

            return dt;
        }




        public void LocalSearchAlgorithm(int[,] solution) 
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            DataTable dt = new DataTable();
            dt = FillDataTable(solution);
            DataRow[] sortedRows = null;

            if(randomNumber < solution.GetLength(0))
            {
                 sortedRows = dt.Select(0.ToString(),randomNumber.ToString());
            }

            
            
          
            


          


        }



    }
}
