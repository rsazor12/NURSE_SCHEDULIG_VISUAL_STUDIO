using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class NurseNavigator
    {
     
        //skladowe dla pierwszej metody
        static sbyte currentWeek = 0;
        static sbyte currentDay = 0;
        static sbyte currentShift = 0;
        static sbyte currentIndexOfNurseOnShift = -1;

        //skladowe pomocnicze do pobierania przedziałami
        static sbyte fromWeek=0;
        static sbyte toWeek=5;
        static sbyte fromDay=0;
        static sbyte toDay=7;
        static sbyte fromShift=0;
        static sbyte toShift=4;

        public static sbyte CurrentWeek { get => currentWeek; set => currentWeek = value; }
        public static sbyte CurrentDay { get => currentDay; set => currentDay = value; }
        public static sbyte CurrentShift { get => currentShift; set => currentShift = value; }
        public static sbyte CurrentIndexOfNurseOnShift { get => currentIndexOfNurseOnShift; set => currentIndexOfNurseOnShift = value; }
        public static sbyte IndexOfNextNurseFromPool { get => indexOfNextNurseFromPool; set => indexOfNextNurseFromPool = value; }

        //skladowe dla drugiej metody
        static sbyte indexOfNextNurseFromPool = -1;

        /// <summary>
        /// Czysci wszytskie skladowe statyczne dla navigacji po Chromosomie: currentDay, CurrentWeek itd.
        /// </summary>
        public static void clearChromosomeStatements()
        {
            CurrentWeek = 0;CurrentDay = 0;CurrentShift = 0;CurrentIndexOfNurseOnShift = -1;
            fromWeek = 0;toWeek = 5;fromDay = 0;toDay = 7;fromShift = 0;toShift = 4;
        }

        public static void clearPoolStatements()
        {
            indexOfNextNurseFromPool = -1;
        }


        /// <summary>
        /// zwraca po kolei pielegniarki z danych zmian z chromosomu
        /// </summary>
        /// <return>NURSE OR NULL</return>
        public static NurseClass getNextNurseFromChromosome(NurseClass[][][][] chromosomeVectorReference) //pomocne dla Constraints ktore pobieraja kazda pielegniarke z chromosomu z kazdej zmiany
        {
            currentIndexOfNurseOnShift++;
           
            if (currentIndexOfNurseOnShift>2)
            {
                currentIndexOfNurseOnShift = 0;
                currentShift++; 
            }
            if(currentShift>=toShift) //jesli shift wyjdzie z tego przedziłu - to zeruj shift
            {
                currentShift = fromShift;
                currentDay++;
            }
            if(currentDay>=toDay)
            {
                currentDay = fromDay;
                currentWeek++;          
            }
            if(currentWeek>=toWeek)
            {
                return null;        //koniec pobierania
            }

            if (chromosomeVectorReference[currentWeek][currentDay][currentShift].Length > currentIndexOfNurseOnShift)
                return chromosomeVectorReference[currentWeek][currentDay][currentShift][currentIndexOfNurseOnShift];

            else
            {
                return getNextNurseFromChromosome(chromosomeVectorReference);
            }
           
        }

        /// <summary>
        /// Zwraca następną pielegniarke z Puli Pielegniarek
        /// </summary>
        /// <returns>nastepna pielegniarka z puli lub NULL - jesli nie istnieje nastepna pielegniarka</returns>
        public static NurseClass getNextNurseFromNursePool(PoolOfNurses obPoolOfNurses)
        {
            indexOfNextNurseFromPool++;
            if (indexOfNextNurseFromPool == PoolOfNurses.sizeOfPool) return null;
            return obPoolOfNurses.getNurseFromPoolFromTheIndex(indexOfNextNurseFromPool);
        }

        public static void setIntervalForReturnedNursesFromChromosome(sbyte fromWeekInterval,sbyte toWeekInterval, sbyte fromDayInterval, sbyte toDayInterval, sbyte fromShiftInterval, sbyte toShiftInterval)
        {
            currentWeek = fromWeek = fromWeekInterval;
            toWeek = toWeekInterval;
            currentDay = fromDay = fromDayInterval;
            toDay = toDayInterval;
            
            currentShift = fromShift = fromShiftInterval;

            toShift = toShiftInterval;
        }
    }


}
