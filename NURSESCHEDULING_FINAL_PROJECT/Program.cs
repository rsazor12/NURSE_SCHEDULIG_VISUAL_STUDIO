using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Constraints.Tests")]


namespace NURSESCHEDULING_FINAL_PROJECT
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.SetBufferSize(200, 100);
            ChromosomeClass obChromosomeClass = new ChromosomeClass();
            //obChromosomeClass.writeNursesFromChromosomeFromEachShift();



            GeneticAlgorithmClass obGeneticAlgorithmClass = new GeneticAlgorithmClass(6,10,20,100,0,4);
            obChromosomeClass = obGeneticAlgorithmClass.runAlgorithm();  //uruchom algorytm , a gdy wsytskie constraints spełnione zwróć najlepszy chromosom

            obChromosomeClass.writeNursesFromChromosomeFromEachShift();  //wypisz ten chromosom ktory jest wynikiem

             Console.WriteLine("\n\nPenalty rozwiązania wynosi \a\a\a\a\a\a" + obChromosomeClass.PenaltyOfChromosome);
            Console.WriteLine("Poziom spełnienia HC : " + obChromosomeClass.checkHowManyHCIsDone());

           



            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
