using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemManagementSim
{
    public class PageReplacementAlgorithms
    {
        public static void FIFO(int num_of_frames, List<int> references)
        {
            int pageFaults = 0;
            Queue<int> queue = new();
            
            foreach (int r in references)
            {
                if (!queue.Contains(r) && queue.Count() < num_of_frames)
                {
                    pageFaults++;
                    queue.Enqueue(r);
                    printSim(r, queue, "PF");
                }
                else if (!queue.Contains(r) && queue.Count() == num_of_frames)
                {
                    pageFaults++;
                    queue.Dequeue();
                    queue.Enqueue(r);
                    printSim(r, queue, "PF");

                }
                else if (queue.Contains(r) && queue.Count() <= num_of_frames)
                {
                    printSim(r, queue, "OK");
                }

            }
            
            printResults(pageFaults, references);

        }
        public static void LRU(int num_of_frames, List<int> references)
        {
           
            int pageFaults = 0;
            Queue<int> queue = new();
            foreach (int r in references)
            {
                if (!queue.Contains(r) && queue.Count() < num_of_frames)
                {
                    pageFaults++;
                    queue.Enqueue(r);
                    printSim(r, queue, "PF");
                }
                else if (!queue.Contains(r) && queue.Count() == num_of_frames)
                {
                    pageFaults++;
                    queue.Dequeue();
                    queue.Enqueue(r);
                    printSim(r, queue, "PF");

                }
                else if (queue.Contains(r) && queue.Count() <= num_of_frames)
                {

                    // na indexno mjesto stavljam onaj sa vrha, a na vrh taj s indexa
                    Queue<int> temp = new();
                    for (int i = 0; i < num_of_frames; i++)
                        temp.Enqueue(queue.ElementAt(i));
                    queue.Clear();
                    foreach (int q in temp)
                        if (!q.Equals(r)) queue.Enqueue(q);
                    queue.Enqueue(r);
                    printSim(r, queue, "OK");
                }

            }

            printResults(pageFaults, references);
        }
        public static void SecondChance(int num_of_frames, List<int> references)
        {
            Console.WriteLine("Select R bit page");
            int R_bit = Convert.ToInt32(Console.ReadLine());
            int R_bit_count = 1;
            int pageFaults = 0;
            Queue<int> queue = new();
            foreach (int r in references)
            {
                if (r == R_bit) R_bit_count = 1;
                if (!queue.Contains(r) && queue.Count() < num_of_frames)
                {
                    pageFaults++;
                    queue.Enqueue(r);
                    printSim(r, queue, "PF");
                }
                else if (!queue.Contains(r) && queue.Count() == num_of_frames)
                {
                    if (queue.Peek() == R_bit && R_bit_count == 1)
                    {
                        Queue<int> temp = new();
                        for (int i = 0; i < num_of_frames; i++)
                            temp.Enqueue(queue.ElementAt(i));
                        queue.Clear();
                        foreach (int q in temp)
                            if (!q.Equals(R_bit)) queue.Enqueue(q);
                        queue.Enqueue(R_bit);
                        queue.Dequeue();
                        queue.Enqueue(r);
                        R_bit_count = 0;
                        printSim(r, queue, "PF");
                        pageFaults++;
                    }
                    else
                    {
                        pageFaults++;
                        queue.Dequeue();
                        queue.Enqueue(r);
                        printSim(r, queue, "PF");
                    }

                }
                else if (queue.Contains(r) && queue.Count() <= num_of_frames)
                {
                    printSim(r, queue, "OK");
                }

            }
            printResults(pageFaults, references);

        }
        public static void LFU(int num_of_frames,int counter,int increaseValue,List<int> references)
        {
            int pageFaults = 0;
            Stack<(int, int)> stack = new Stack<(int, int)>();
            foreach (int r in references)
            {

                if (!ContainsFrame(stack,r) && stack.Count() < num_of_frames)
                {
                    pageFaults++;
                    Stack<(int, int)> temp = new();
                    foreach(var s in stack) temp.Push(s);
                    stack.Clear();
                    foreach(var s in temp)
                        stack.Push((s.Item1,s.Item2-1));
                    stack.Push((r,counter));
                    stack = Sort(stack,'<');
                    printSim(r, stack, "PF");
                }
                else if (!ContainsFrame(stack,r) && stack.Count() == num_of_frames)
                {
                    pageFaults++;
                    stack = Sort(stack,'>');
                    stack.Pop();
                    Stack<(int, int)> temp = new();
                    foreach (var s in stack) temp.Push(s);
                    stack.Clear();
                    foreach (var s in temp)
                        stack.Push((s.Item1, s.Item2 - 1));
                    stack.Push((r,counter));
                   stack=Sort(stack,'<');
                    printSim(r, stack, "PF");

                }
                else if (ContainsFrame(stack,r) && stack.Count() <= num_of_frames)
                {
                    Stack<(int, int)> temp = new(); 
                    foreach(var s in stack) temp.Push(s);
                    stack.Clear();
                    (int,int) incVal;
                    foreach(var t in temp)
                    {
                        if (t.Item1 == r) { incVal = (t.Item1, t.Item2 + increaseValue); stack.Push(incVal); }
                        else
                            stack.Push((t.Item1,t.Item2-1));

                    }
                    stack = Sort(stack,'<');
                    printSim(r, stack, "OK");
                }

            }
            printResults(pageFaults, references);

        }
        public static void Optimal(int num_of_frames, List<int> references)
        {
            int pageFaults = 0;
            Queue<int> queue = new();
            Queue<int> rq = new();
            foreach(int i in references) rq.Enqueue(i);
            foreach (int r in references.ToList())
            {
                rq.Dequeue();
                if (!queue.Contains(r) && queue.Count() < num_of_frames)
                 {
                   
                        pageFaults++;
                        queue.Enqueue(r);
                        printSim(r, queue, "PF");

                }
                    else if (!queue.Contains(r) && queue.Count() == num_of_frames)
                    {
                    
                        pageFaults++;
                        //prediciting the longest non-referencing one
                        List<(int, int)> list = new List<(int, int)>(); int furthest = -1; 
                        foreach (int q in queue)
                        {
                            if (!rq.Contains(q)) { furthest = q; break; }
                            else list.Add((q, returnIndex(rq,q)));
                        }
                        if (furthest == -1)
                        {
                            (int, int) x = list.MaxBy(x => x.Item2);
                            furthest = x.Item1;
                        }

                        Queue<int> temp = new();
                        foreach (int q in queue) temp.Enqueue(q);
                        queue.Clear();
                        foreach (var t in temp)
                            if (t == furthest) queue.Enqueue(r);
                            else queue.Enqueue(t);

                        printSim(r, queue, "PF");

                    }
                    else if (queue.Contains(r) && queue.Count() <= num_of_frames)
                    {

                            printSim(r, queue, "OK");

                    }

             }

            printResults(pageFaults, references);
            
        }
        private static void printResults(int pageFaults,List<int> references)
        {
            double efficiency = ((double)pageFaults / (double)references.Count()) * 100;
            Math.Round(efficiency, 2);
            Console.WriteLine("Algorithm efficiency: PF=" + pageFaults + "\t=>\tpf = " + pageFaults + " / " + references.Count() + " = " + efficiency + "%");
        }
        private static void printSim(int r, Queue<int> queue, string s)
        {
           
            Console.Write(r + "\t");
            Console.Write(s + "\t");
            for (int i = queue.Count()-1; i >= 0; i--) Console.Write(queue.ElementAt(i) + "\t");
            Console.WriteLine();
        }
        private static void printSim(int r, Stack<(int, int)> stack, string v)
        {
            Console.Write(r + "\t");
            Console.Write(v + "\t");
            foreach (var i in stack) Console.Write(i + "\t");
            Console.WriteLine();
        }

        private static Stack<(int,int)> Sort(Stack<(int,int)> stack,char criteria)
        {
            Stack<(int,int)> temp = new Stack<(int,int)>();
       
            while (stack.Count > 0)
            {
                var element = stack.Pop();
    
                if(criteria=='<')
                    while (temp.Count > 0 && (element.Item2 < temp.Peek().Item2))
                    {
                        stack.Push(temp.Pop());
                    }
                else while (temp.Count > 0 && (element.Item2 > temp.Peek().Item2))
                    {
                        stack.Push(temp.Pop());
                    }

                temp.Push(element);
            }
            return temp;
        }
     
        private static bool ContainsFrame(Stack<(int, int)> stack, int r)
        {
            foreach (var x in stack)
                if (x.Item1 == r)
                    return true;
            return false;
        }
        private static int returnIndex(Queue<int> x, int y)
        {
            int res=0;
            for (int i = 0; i < x.Count; i++)
                if (x.ElementAt(i) == y) { res = i; break; }
            return res;
               
        }
    }
}


