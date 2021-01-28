using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace pMedian
{
    public class Chromosome
    {
        //public List<Location> locations;
        public int[,] solution = null;
        public int chromosomeFitness = 0;
        public List<int> warehouses;
        public double selectionFitness = 0;
        public Chromosome()
        {
        }

        public void BuildChromosome(FileHandler inputFile, int seed) 
        {
            
            Random rnd = new Random(seed);
            // generate p random ints between number of nodes and store it in a list

            warehouses = new List<int>();

            
            for (int index = 0; index < inputFile.p; index++)
            {
                int newWarehouse = rnd.Next(0, inputFile.nodes);

                // Check if last added number is already in the list
                while (warehouses.Contains(newWarehouse) == true)
                {
                    newWarehouse = rnd.Next(0, inputFile.nodes); 
                }

                warehouses.Add(newWarehouse);
            }

            // sort the list
            warehouses.Sort();

            // create x_ij matrix where rows which are not warehouses can only contain 0es and warehouses are not connected to another warehouse or themselves
            // i are columns j are rows
            solution = Algorithm.AssignCustomersToWarehouses(this,inputFile);
            
            chromosomeFitness = FitnessValue(inputFile);
            //return chromosome;

            
        }

        public int FitnessValue(FileHandler fh)
        {
            int objValue = 0;
            for (int i = 0; i < fh.nodes; i++)
            {
                for (int j = 0; j < fh.nodes; j++)
                {
                    //Console.WriteLine(i + " " + j);
                    objValue += fh.cost[i, j] * solution[i, j];
                }
            }



            return objValue;
        }

        

    }
}
