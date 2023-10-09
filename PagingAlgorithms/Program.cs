using System;
using static MemManagementSim.PageReplacementAlgorithms;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
           int numOfFrames=0;
           int numOfReferences; 
           List<int> references = new(); 
           string[] nums = {""};
           int[] ints;
           List<string> alg = new();
            
            try
            {
                Console.Write("Type in number of frames ");
                numOfFrames = Convert.ToInt32(Console.ReadLine());
                Console.Write("Type in number of references: ");
                numOfReferences = Convert.ToInt32(Console.ReadLine());
                Console.Write("Type in references: ");
                nums = Console.ReadLine().Split(',');
                ints = Array.ConvertAll(nums, s => int.Parse(s));
                references.AddRange(ints);
                Console.Write("Choose algorithms: ");
                
                alg.AddRange(Console.ReadLine().Split(','));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            
            foreach (string a in alg)
            {
                switch (a)
                {
                    case "FIFO":
                       
                        Console.WriteLine("Simulation results:\nFIFO");
                        FIFO(numOfFrames, references);
                        break;
                    case "LRU":
                        Console.WriteLine("Simulation results:\nLRU");
                        LRU(numOfFrames, references);
                        break;
                    case "Second Chance":
                        Console.WriteLine("Simulation results:\nSecond Chance");
                        SecondChance(numOfFrames, references);
                        break;
                    case "LFU":
                        Console.WriteLine("AGING MECHANISM:");
                        Console.WriteLine("Type in initial counter value:");
                        int initCounter = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Choose increment value:");
                        int inc = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Simulation results:\nLFU");
                        LFU(numOfFrames, initCounter, inc, references);

                        break;
                    case "Optimal":
                        Console.WriteLine("Simulation results:\nOptimal");
                        Optimal(numOfFrames, reference);
                        break;
                    default:
                        Console.WriteLine("Error!");
                        Console.WriteLine("Typo ->" + (alg.IndexOf(a) + 1) + "." + "algorithm!");
                        break;



                }
            }

        }
            
    }
}
