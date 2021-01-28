using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace pMedian
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int seed = rnd.Next();
            FileHandler file = new FileHandler();

            Chromosome solution = GeneticAlgorithm(file, seed);

            Console.WriteLine("Best Solution: " + solution.chromosomeFitness);


            /*
            Generation generation = new Generation();
            generation.buildInitialGeneration(file, 4, seed);
            

            (Chromosome, Chromosome) selection = Algorithm.Selection(generation);
            Chromosome crossoverChromosome = Algorithm.Crossover(selection.Item1, selection.Item2, file, seed).Item1;

            crossoverChromosome.solution = Algorithm.AssignCustomersToWarehouses(crossoverChromosome, file);

            PrintMatrix(crossoverChromosome.solution);
            */
            /*
            //PrintMatrix(file.cost);
            //Chromosome chromosome = new Chromosome();
            //chromosome.BuildChromosome(file,1);
            //PrintMatrix(chromosome.solution);
            Generation generation = new Generation();
            generation.buildInitialGeneration(file, 3, 53);

            Console.WriteLine(generation.chromosomes.Count());
            //PrintMatrix(generation.chromosomes[0].solution);
            

            Algorithm algorithm = new Algorithm();
            (Chromosome, Chromosome) selection = algorithm.Selection(generation);
            algorithm.Mutation(selection.Item1);

            //priority Test
            Chromosome testChromosome = algorithm.AssignCustomersToWarehouses(generation.chromosomes[0], file);
            PrintMatrix(testChromosome.solution);
            Console.WriteLine(testChromosome.chromosomeFitness);
            */

        }

        public static Chromosome GeneticAlgorithm(FileHandler file, int seed)
        {
            Generation generation = new Generation();
            generation.buildInitialGeneration(file, 10, seed);
            int thisGen = 0;
            int bestGen = 0;
            Chromosome bestSolution = null;
            int bestSolutionValue = 100000;
            Random rnd = new Random(seed);

            //LocalSearch ls = new LocalSearch();

            while ((thisGen - bestGen) < 10)
            {
                thisGen += 1;

                if(generation.BestSolution().chromosomeFitness < bestSolutionValue)
                {
                    //bestSolution = generation.BestSolution();
                    //bestSolutionValue = bestSolution.chromosomeFitness;
                    //bestGen = thisGen;
                    //Console.WriteLine("New Best Solution: " + bestSolution.chromosomeFitness);
                    bestSolution = new Chromosome();
                    bestSolution.warehouses = generation.BestSolution().warehouses;
                    bestSolution.solution = generation.BestSolution().solution;
                    bestSolution.chromosomeFitness = bestSolution.FitnessValue(file);
                    bestSolutionValue = bestSolution.chromosomeFitness;
                    bestGen = thisGen;
                }

                //(Chromosome, Chromosome) parents = Algorithm.Selection(generation);
                //(Chromosome, Chromosome) offspring = Algorithm.Crossover(parents.Item1, parents.Item2, file, seed);

                Chromosome parent1 = Algorithm.Select(generation, seed);
                Chromosome parent2 = Algorithm.Select(generation, seed+1);
                Console.WriteLine("Parent 1: " + generation.chromosomes.IndexOf(parent1));
                Console.WriteLine("Parent 2: " + generation.chromosomes.IndexOf(parent2));
                (Chromosome, Chromosome) offspring = Algorithm.Crossover(parent1, parent2, file, seed);

                offspring.Item1.solution = Algorithm.AssignCustomersToWarehouses(offspring.Item1, file);
                offspring.Item2.solution = Algorithm.AssignCustomersToWarehouses(offspring.Item2, file);
                offspring.Item1.chromosomeFitness = offspring.Item1.FitnessValue(file);
                offspring.Item2.chromosomeFitness = offspring.Item2.FitnessValue(file);

                if (rnd.Next(100) < 5)
                    offspring.Item1 = Algorithm.Mutation(offspring.Item1, file);
                if (rnd.Next(100) < 5)
                    offspring.Item2 = Algorithm.Mutation(offspring.Item2, file);

                //offspring.Item1.solution = ls.shuffleSolution(offspring.Item1.solution);
                //offspring.Item2.solution = ls.shuffleSolution(offspring.Item2.solution);

                offspring.Item1 = Algorithm.LocalSearch(offspring.Item1, file);
                offspring.Item2 = Algorithm.LocalSearch(offspring.Item2, file);

                (int, int) indicesToReplace = Algorithm.GetIndOfUnfittest(generation);
                generation.chromosomes[indicesToReplace.Item1] = offspring.Item1;
                generation.chromosomes[indicesToReplace.Item2] = offspring.Item2;

                Console.WriteLine("Current Gen: " + thisGen + ", Best Gen: " + bestGen);
            }
            return bestSolution;
        }


        public static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i,j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
