using Lab3.SkipList;
using Lab6.BinomialTree;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Demonstration
{
    class Program
    {
        static void Main(string[] args)
        {
            var binomialHeap = new BinomialHeap<int>(int.MinValue, Comparer<int>.Default);
          
            while (true)
            {
                try
                {
                    binomialHeap.Display();
                    Instruct();
                    var operationKey = Console.ReadKey();
                    Console.WriteLine();
                    switch (operationKey.Key)
                    {
                        case (ConsoleKey.D1):
                            {
                                Console.WriteLine("Input number: ");
                                var number = Console.ReadLine();

                                if (int.TryParse(number, out int resValue))
                                {
                                    binomialHeap.Insert(resValue);
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect number");
                                }
                                break;
                            }
                        case (ConsoleKey.D2):
                            {
                                binomialHeap.ExtractMin();
                                break;
                            }
                        case (ConsoleKey.D3):
                            {
                                Console.WriteLine("Size: " + binomialHeap.Size);
                                Console.ReadKey();
                                break;
                            }
                        case (ConsoleKey.D4):
                            {
                                var countOfTrees = binomialHeap.CountTrees();
                                Console.WriteLine("Count of Trees: (int)-" + countOfTrees.Item1 + " (binary)-" + countOfTrees.Item2);
                                Console.ReadKey();
                                break;
                            }
                        case (ConsoleKey.D5):
                            {
                                binomialHeap.Clear();
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }

                Console.Clear();
            }
        }
        private static void Instruct()
        {
            Console.WriteLine("Choose the operation for Matrix: ");
            Console.WriteLine("1. Insert element");
            Console.WriteLine("2. Extract min");
            Console.WriteLine("3. Get Size");
            Console.WriteLine("4. Get Binomial Trees Count");
            Console.WriteLine("5. Clear");
        }
    }
}
