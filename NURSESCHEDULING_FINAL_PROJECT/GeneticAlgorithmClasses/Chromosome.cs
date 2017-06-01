using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NURSESCHEDULING_FINAL_PROJECT.GeneticAlgorithmClasses;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class ChromosomeClass
    {
        PoolOfNurses obPoolOfNurses;
        //tworze regularna tablice (nie postrzepiona) na pielegniarki
        NurseClass[,,,] chromosomeVector = new NurseClass[5,7,4,3]; //odwolanie: ob[week][day][shift][referenceOfNurse}

        AbstractConstraintsClass obConstraintsClass;

        public ChromosomeClass()
        {
            obPoolOfNurses = new PoolOfNurses();

            init(); // wypelniam wektor losowymi wartosciami
        }


        private void init() //tu poprostu inicjuje stworzony wczesniej wektor na pielegniarki
        {
            for(int week=0;week<5;week++)
            {
                for(int day=0;day<7;day++)
                {
                    for(int shift=0;shift<4;shift++)
                    {
                        for(int indexOfNurse=0;indexOfNurse<3;indexOfNurse++)
                        {
                            chromosomeVector[week,day,shift,indexOfNurse] = null; 
                        }
                    }

                }
            }
        }
        public void writeOnConsole()
        {
            //najpierw wypisuje literki zmian odpowiednio e d l n e d l n itd..

            for (int i=0;i<16;i++)
            {
                Console.SetWindowSize(Console.LargestWindowWidth,Console.LargestWindowHeight);
               
                Console.Write("e  d  l  n |");
            }
            Console.Write("\n");
            for (int i = 0; i < 16; i++)
            {
               
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                Console.Write("e  d  l  n |");
            }
            Console.Write("\n");
            for (int i = 0; i < 3; i++)
            {

                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                Console.Write("e  d  l  n |");
            }

        }
    }
}
