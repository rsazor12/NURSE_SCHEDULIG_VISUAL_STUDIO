using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class PoolOfNurses
    {
        List<NurseClass> listOfNurses;  // dodaje pielegnirki do listy
        public static int sizeOfPool=16;       //ta wartosc bedzie odczytywana pozniej z bazy 
        public static int countOfFullTimeNurses = 14;
        public static int countOfPartTimeNurses = 2;

        //skłądowe pomocne do pobierania pielegniarek
        static int currentNurseId = -1;
        
        public static void clearStaticCounter()
        {
            currentNurseId = -1;
        }

        public PoolOfNurses()
        {
            listOfNurses = new List<NurseClass>();

            //musze utworzyc Pielegniarki i dodac je do listy 
            for(sbyte i=0;i<sizeOfPool;i++)
            {
                //pierwsze iles tam to fulltime
                if(i<countOfFullTimeNurses)
                listOfNurses.Add(new NurseClass(i.ToString(),i.ToString(),NurseClass.KindOfJob.fulltime)); //imie i nazwisko to narazie liczba !! id tworzy sie samo w NurseClass
                else
                    listOfNurses.Add(new NurseClass(i.ToString(), i.ToString(), NurseClass.KindOfJob.parttime)); //reszta to part time
            }
        }

        public NurseClass getNurseFromPoolFromTheIndex(sbyte index)
        {
            return listOfNurses[index];
        }

        public NurseClass getRandomNurseFromPoolAndRemoveNurse()
        {
            Random rnd = new Random();
            sbyte indexOfRandomNurse;

            if (listOfNurses.Count>2)
            {
                indexOfRandomNurse = (sbyte)(rnd.Next(1, listOfNurses.Count) - 1);
            }
            else
            {
                indexOfRandomNurse = 0;
            }
           
            NurseClass selectedNurse = listOfNurses[indexOfRandomNurse];
                listOfNurses.RemoveAt(indexOfRandomNurse);
            return selectedNurse;
        }

        Random rnd = new Random();

        public static int CurrentNurseId { get => currentNurseId; set => currentNurseId = value; }
        public static int CurrentNurseId1 { get => currentNurseId; set => currentNurseId = value; }

        public NurseClass getRandomNurseFromPool()
        {
            sbyte indexOfRandomNurse;

            if (listOfNurses.Count > 2)
            {
                indexOfRandomNurse = (sbyte)(rnd.Next(1, listOfNurses.Count) - 1);
            }
            else
            {
                indexOfRandomNurse = 0;
            }

            NurseClass selectedNurse = listOfNurses[indexOfRandomNurse];
            
            return selectedNurse;
        }
        /// <summary>
        /// zwraca po kolei pielegniarki z Puli , po 16 pielegniarce zwracana jest ta z indeksem 1
        /// </summary>
        /// <returns></returns>
        public NurseClass getNurseFromPoolByOrder()
        {
            currentNurseId++;
            if (currentNurseId == sizeOfPool )
                currentNurseId = 0;
            return listOfNurses[currentNurseId];
        }
       


    }
}
