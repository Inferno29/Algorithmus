using System;
using System.IO;

namespace pMedian
{
    public class FileHandler
    {
        public int nodes { get; set; }
        public int p { get; set; }
        public int[,] cost { get; set; }
        public FileHandler()
        {
            nodes = -1;
            p = -1;
            cost = null;
            Read();
        }

        public void Read()
        {
            //reading the file
            string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = projectFolder + @"\pmed\pmed1.txt";
            string[] lines = File.ReadAllLines(path);
            bool firstLine = true;
            foreach (string line in lines)
            {
                string[] elements = line.Split(' ');
                if (firstLine)
                {
                    nodes = Convert.ToInt32(elements[1]);
                    p = Convert.ToInt32(elements[3]);
                    cost = new int[nodes, nodes];

                    // filling the cost array with default values
                    for (int i = 0; i < nodes; i++)
                        for (int j = 0; j < nodes; j++)
                        {
                            if (i == j)
                                cost[i, j] = 0;
                            else
                                cost[i, j] = 1000;
                        }

                    firstLine = false;
                }
                else
                {
                    // assigning the given edges to the respective elements in the array; subtracting 1 because vertices start with 1 but array indices start with 0;
                    int n1 = Convert.ToInt32(elements[1]) - 1;
                    int n2 = Convert.ToInt32(elements[2]) - 1;
                    int c = Convert.ToInt32(elements[3]);
                    cost[n1, n2] = c;
                    cost[n2, n1] = c;
                   // Console.WriteLine(c);
                }
            }

            //Floyd's algorithm
            for (int k = 0; k < nodes; k++)
                for (int i = 0; i < nodes; i++)
                    for (int j = 0; j < nodes; j++)
                        if (cost[i, j] > cost[i, k] + cost[k, j])
                            cost[i, j] = cost[i, k] + cost[k, j];
        }
    }
}