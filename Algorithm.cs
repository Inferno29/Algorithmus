using System;
using System.Collections.Generic;

namespace pMedian
{
	public static class Algorithm
	{
		static public (Chromosome, Chromosome) Selection(Generation gen)
        {
			SortedList<double, Chromosome> chromosomeMap = new SortedList<double, Chromosome>();

			foreach (Chromosome chrom in gen.chromosomes)
            {
				//Console.WriteLine("Fitness: " + chrom.chromosomeFitness);
				if(chromosomeMap.ContainsKey(chrom.chromosomeFitness))
					chromosomeMap.Add(chrom.chromosomeFitness + 0.001*gen.chromosomes.IndexOf(chrom), chrom);
				else
					chromosomeMap.Add(chrom.chromosomeFitness, chrom);
            }

			return (chromosomeMap.Values[0], chromosomeMap.Values[1]);
        }

		static public (int, int) GetIndOfUnfittest(Generation gen)
		{
			SortedList<double, Chromosome> chromosomeMap = new SortedList<double, Chromosome>();

			foreach (Chromosome chrom in gen.chromosomes)
			{
				if (chromosomeMap.ContainsKey(chrom.chromosomeFitness))
					chromosomeMap.Add(chrom.chromosomeFitness + 0.001 * gen.chromosomes.IndexOf(chrom), chrom);
				else
					chromosomeMap.Add(chrom.chromosomeFitness, chrom);
			}

			return (gen.chromosomes.IndexOf(chromosomeMap.Values[chromosomeMap.Count-2]), gen.chromosomes.IndexOf(chromosomeMap.Values[chromosomeMap.Count-1]));
		}

		public static Chromosome Mutation(Chromosome chrom, FileHandler inputFile) //assign 1 random customer to a different warehouse
        {
			Random rnd = new Random();

			int warehouse = -1;
			int newWarehouse = -1;

			// find a random warehouse
			warehouse = chrom.warehouses[rnd.Next(0, chrom.warehouses.Count - 1)];

			while (true) //find a random node, which is not a warehouse
            {
                newWarehouse = rnd.Next(0, inputFile.nodes - 1);
                if (!chrom.warehouses.Contains(newWarehouse))
                    break;
            }

			// overwrite a random existing warehouse with the new one
			chrom.warehouses[rnd.Next(0, chrom.warehouses.Count - 1)] = newWarehouse;

			// reassign customers to the new warehouses and recalculate fitness value
			chrom.solution = AssignCustomersToWarehouses(chrom, inputFile);
			chrom.chromosomeFitness = chrom.FitnessValue(inputFile);

            //Random rnd = new Random();
            //int cust = -1;
            //int warehouse = -1;
            //int newWarehouse = -1;

            //while (true) //find a random node, which is not a warehouse
            //         {
            //	cust = rnd.Next(0, (int)Math.Sqrt(chrom.solution.Length) - 1);
            //	if (!chrom.warehouses.Contains(cust))
            //		break;
            //         }

            //foreach (int i in chrom.warehouses) //find the warehouse, this node is assigned to
            //	if (chrom.solution[cust, i] == 1)
            //		warehouse = i;

            //while (true) // find a random warehouse, which is different from the current one
            //{
            //	newWarehouse = chrom.warehouses[rnd.Next(0, chrom.warehouses.Count - 1)];
            //	if (!(newWarehouse==warehouse))
            //		break;
            //}

            //Console.WriteLine(warehouse + " " + newWarehouse + " " + cust);
            //// reassign the customer from the old warehouse to the new one
            //chrom.solution[cust, warehouse] = 0;
            //chrom.solution[cust, newWarehouse] = 1;

            return chrom;
        }

		public static int[,] AssignCustomersToWarehouses(Chromosome initialChromosome, FileHandler inputFile)
		{
			// get List to store the best warehouse for each location
			List<int> warehousesToUse = new List<int>();

			// initialize new chromosome for output
			Chromosome newChromosome = new Chromosome();

			// iterate and initialize cost and inicies for warehouses for every i
			for (int i = 0; i < inputFile.nodes; i++)
			{
				int bestWarehouseCost = int.MaxValue;
				int bestWarehouseIndex = int.MinValue;
				
				// iterate through every warehouse for every location and select the best one
				for (int j = 0; j < initialChromosome.warehouses.Count; j++)
				{
					if (inputFile.cost[i, initialChromosome.warehouses[j]] < bestWarehouseCost)
					{
						bestWarehouseIndex = initialChromosome.warehouses[j];
						bestWarehouseCost = inputFile.cost[i, initialChromosome.warehouses[j]];

					}
					
				}
				// write the index of every best warehouse per i in a list to assign them in the next step
				warehousesToUse.Add(bestWarehouseIndex);
			}

			// create x_ij matrix where rows which are not warehouses can only contain 0es and warehouses are not connected to another warehouse or themselves
			// i = column, j = row

			newChromosome.solution = new int[inputFile.nodes, inputFile.nodes];

			//iterate through columns
			for (int i = 0; i < inputFile.nodes; i++)
			{
				// ask if the current column is a warehouse. If true do not make a connection to a warehouse
				if (initialChromosome.warehouses.Contains(i))
				{
					for (int j = 0; j < inputFile.nodes; j++)
					{
						newChromosome.solution[i, j] = 0;
					}
				}

				// iterate through the rows and if the row has the same index as the prior chosen warehouse x_ij = 1
				else
				{
					for (int j = 0; j < inputFile.nodes; j++)
					{
						if (j == warehousesToUse[i])
						{
							newChromosome.solution[i, j] = 1;
						}

						else
						{
							newChromosome.solution[i, j] = 0;
						}
					}
				}
			}
			// get other data for the new Chromosome
			//newChromosome.chromosomeFitness = newChromosome.FitnessValue(inputFile);
			//newChromosome.warehouses = initialChromosome.warehouses;
			return newChromosome.solution;
		}

		public static (Chromosome, Chromosome) Crossover(Chromosome parent1, Chromosome parent2, FileHandler inputFile, int seed)
		{
			parent1.warehouses.Sort();
			parent2.warehouses.Sort();
			if (parent1.warehouses == parent2.warehouses)
			{
				Console.WriteLine("FUCK!U");
			}
			Random rnd = new Random(seed);
			// Create exchange vector for each parent
			List<int> exchangeVectorParent1 = new List<int>();
			List<int> exchangeVectorParent2 = new List<int>();

			// fill exchange vector for parent1 and parent 2 simultaneously
			Console.WriteLine("nParent1.warehouses = " + parent1.warehouses.Count);
			Console.WriteLine("nParent2.warehouses = " + parent2.warehouses.Count);
			for (int i = 0; i < inputFile.p; i++)
			{
				if (parent2.warehouses.Contains(parent1.warehouses[i]) == false)
				{
					exchangeVectorParent1.Add(parent1.warehouses[i]);
				}

				if (parent1.warehouses.Contains(parent2.warehouses[i]) == false)
				{
					exchangeVectorParent2.Add(parent2.warehouses[i]);
				}
			}

			// If the warehouses of parent 1 and 2 are not completeley the same perform a crossover (if not do not perform a crossover)

			int exchangeVectorLength = exchangeVectorParent1.Count;
			Console.WriteLine("exchangeVectorLength Parent 1: " + exchangeVectorParent1.Count);
			Console.WriteLine("exchangeVectorLength Parent 2: " + exchangeVectorParent2.Count);
			Console.WriteLine("exchangeVectorLength = " + exchangeVectorLength);
			if (exchangeVectorLength > 0)
			{
                // Randomly select the position in which the crossover (swap) will take place (making sure that at least 1 warehouse will be swapped)

                int swapPosition = 1;
                if (exchangeVectorLength > 1)
                {
                    swapPosition = rnd.Next(1, exchangeVectorLength - 1);
                }
                Console.WriteLine("swapPosition = " + swapPosition);

                // Create Children 1 and 2
                List<int> warehousesChild1 = new List<int>();
				List<int> warehousesChild2 = new List<int>();

				////Fill children with the parent warehouses until swapPosition.
				//for (int i = 0; i < swapPosition; i++)
				//{
				//	warehousesChild1.Add(parent1.warehouses[i]);
				//	warehousesChild2.Add(parent2.warehouses[i]);
				//}

				//// Fill the remaining spaces with the exchange Vectors
				//for (int i = 0; i < inputFile.p - swapPosition; i++)
				//{
				//	//if(i < exchangeVectorLength)
				// //               {
				//	//	warehousesChild1.Add(exchangeVectorParent2[i]);
				//	//	warehousesChild2.Add(exchangeVectorParent1[i]);
				//	//}
				//	warehousesChild1.Add(exchangeVectorParent2[i]);
				//	warehousesChild2.Add(exchangeVectorParent1[i]);
				//}
				Console.WriteLine("Exchange Length: " + exchangeVectorLength);
				foreach(int warehouse in parent1.warehouses)
					if(exchangeVectorParent1.Contains(warehouse) == false)
                    {
						warehousesChild1.Add(warehouse);
						warehousesChild2.Add(warehouse);
					}

                //Fill children with the parent warehouses until swapPosition.
                for (int i = 0; i < swapPosition; i++)
                {
                    warehousesChild1.Add(exchangeVectorParent1[i]);
                    warehousesChild2.Add(exchangeVectorParent2[i]);
                }

                // Fill the remaining spaces with the exchange Vectors
                for (int i = swapPosition; i < exchangeVectorLength; i++)
                {
                    //if(i < exchangeVectorLength)
                    //               {
                    //	warehousesChild1.Add(exchangeVectorParent2[i]);
                    //	warehousesChild2.Add(exchangeVectorParent1[i]);
                    //}
                    warehousesChild1.Add(exchangeVectorParent2[i]);
                    warehousesChild2.Add(exchangeVectorParent1[i]);
                }

                // Create Children Chromosomes
                Chromosome Child1 = new Chromosome();
				Child1.warehouses = warehousesChild1;
				Chromosome Child2 = new Chromosome();
				Child2.warehouses = warehousesChild2;
				Console.WriteLine("Number of Warehouses Child1 = " + warehousesChild1.Count);
				Console.WriteLine("Number of Warehouses Child2 = " + warehousesChild2.Count);

				// Return children
				return (Child1, Child2);
			}

			// Else keep one parent and create a new random one. (Such that no duplicates are stored in the new Generation)
			else
			{
				//Chromosome Child1 = parent1;
				Chromosome Child1 = new Chromosome();
				//Child1.warehouses = parent1.warehouses;
				//Child1.solution = parent1.solution;
				Child1.BuildChromosome(inputFile, seed + 1);
				Child1.chromosomeFitness = Child1.FitnessValue(inputFile);
				Chromosome Child2 = new Chromosome();
				Child2.BuildChromosome(inputFile, seed);
				Child2.chromosomeFitness = Child2.FitnessValue(inputFile);

				//for (int index = 0; index < inputFile.p; index++)
				//{
				//	int newWarehouse = rnd.Next(0, inputFile.nodes);

				//	// Check if last added number is already in the list
				//	while (Child2.warehouses.Contains(newWarehouse) == true)
				//	{
				//		newWarehouse = rnd.Next(0, inputFile.nodes);
				//	}

				//	Child2.warehouses.Add(newWarehouse);
				//}
				// Return children
				return (Child1, Child2);
			}
		}

		public static Chromosome LocalSearch(Chromosome chrom, FileHandler file)
		{
			bool ongoing = true;
			while (ongoing)
			{
				ongoing = false;
				for (int w = 0; w < file.p; w++)
					for (int c = 0; c < file.nodes; c++)
						if (chrom.warehouses.Contains(c) == false)
						{
							Chromosome newChrom = new Chromosome();
							newChrom.warehouses = chrom.warehouses;
							newChrom.warehouses[w] = c;
							newChrom.solution = Algorithm.AssignCustomersToWarehouses(newChrom, file);
							newChrom.chromosomeFitness = newChrom.FitnessValue(file);
							if (newChrom.chromosomeFitness < chrom.chromosomeFitness)
							{
								chrom = newChrom;
								ongoing = true;
							}
						}
			}
			return chrom;
		}

		public static Chromosome Select(Generation generation, int seed)
		{
			generation.CalculateTotalGenerationFitness();
			// Assign fitness values for the selection process

			double totalFitness = 0;

			foreach  (var chromosome in generation.chromosomes)
			{
				chromosome.selectionFitness = 1 - (chromosome.chromosomeFitness / generation.generationFitness);
				totalFitness += chromosome.selectionFitness;
			}

			foreach (var chromosome in generation.chromosomes)
			{
				chromosome.selectionFitness = chromosome.selectionFitness / totalFitness;
			}

			// Initialize (convert fitness to selection wheel values (Cumulation))
			//generation.CalculateTotalGenerationFitness();

			Random rng = new Random();

			//int index = rng.Next(0, generation.chromosomes.Count);

			//int randomNumber = rng.Next(100);
			double randomNumber = rng.NextDouble();


            //Chromosome selected = new Chromosome();

            // Iterate through the "selection wheel"(cumulated fitnesses)
            // until the random number is not bigger as the cumulated fitness anymore.
            foreach (Chromosome chrom in generation.chromosomes)
            {

				randomNumber -= chrom.selectionFitness;
                if (randomNumber <= 0)
                {
                    return chrom;
                }
            }

            return null;
            //return generation.chromosomes[index];
		}
	}
}

