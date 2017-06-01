using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class ShiftNavigator
    {
        public enum ShiftLengthEnum
        {
            early = 9,
            day = 9,
            late = 8,
            night = 9
        }

        public enum KindOfShift
        {
            early,
            day,
            late,
            night
        }

        public  enum DaysOfWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        static ShiftLengthEnum currentShiftLength; //
        static KindOfShift currentKindOfShift;

        internal static KindOfShift CurrentKindOfShift { get => currentKindOfShift; set => currentKindOfShift = value; }

        /// <summary>
        /// Zwraca dlugosc zmiany , ktora jest aktualnie w NurseNavigator
        /// </summary>
        public static ShiftLengthEnum getCurrentShiftLengthForNurseFromNurseNavigator()
        {
            if (NurseNavigator.CurrentShift == 0)
            {
                currentShiftLength = ShiftLengthEnum.early;
                currentKindOfShift = KindOfShift.early;
                return currentShiftLength;
            }
            if (NurseNavigator.CurrentShift == 1)
            {
                currentKindOfShift = KindOfShift.day;
                return currentShiftLength = ShiftLengthEnum.day;
            }
            if (NurseNavigator.CurrentShift == 2)
            {
                currentKindOfShift = KindOfShift.late;
                return currentShiftLength = ShiftLengthEnum.late;
            }
            if (NurseNavigator.CurrentShift == 3)
            {
                currentKindOfShift = KindOfShift.night;
                return currentShiftLength = ShiftLengthEnum.night;
            }

            

            return 0;
        }
    }
}
          
            
