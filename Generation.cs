using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace pMedian
{
    public class Generation
    {
        public List<Chromosome> chromosomes;
        public double generationFitness { get; set; }
        public Generation()
        {
            chromosomes = new List<Chromosome>();
        }

        public void CalculateTotalGenerationFitness()
        {
            generationFitness = 0;
            foreach(var chromosome in chromosomes)
            {
                generationFitness += chromosome.chromosomeFitness;
            }
        }

        public void buildInitialGeneration(FileHandler inputFile, int numberOfChromosomes, int seed)
        {
            chromosomes = new List<Chromosome>();
            Random rnd = new Random(seed);
            Generation generation = new Generation();
            for (int node = 0; node < numberOfChromosomes; node++)
            {
                Chromosome newChromosome = new Chromosome();
                newChromosome.BuildChromosome(inputFile, rnd.Next());
                chromosomes.Add(newChromosome); //Error null
                Console.WriteLine(newChromosome.solution[0,0]);
            }
        }

        public Chromosome BestSolution() // find the best solution in this generation
        {
            Chromosome bestSolution = null;
            int bestValue = 10000000;

            foreach(Chromosome sol in chromosomes)
                if(sol.chromosomeFitness < bestValue)
                {
                    bestSolution = sol;
                    bestValue = sol.chromosomeFitness;
                }

            Console.WriteLine("Best fitness: " + bestSolution.chromosomeFitness);
            return bestSolution;
        }
    }
}
