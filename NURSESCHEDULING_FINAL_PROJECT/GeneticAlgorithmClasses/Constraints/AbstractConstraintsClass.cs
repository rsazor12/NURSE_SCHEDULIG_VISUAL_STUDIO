using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NURSESCHEDULING_FINAL_PROJECT;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    abstract class AbstractConstraintsClass
    {
		public  NurseClass[][][][] chromosomeVectorReference; //referencja do Chromosoma tylko do wektora nie całej klasy
        public PoolOfNurses obPoolOfNursesReference;

        //zdarzenia które będą powiadamiać o spełnieniu odpowiednich Constraints
        public delegate void HC1Delegate(int whichConstraintDone);
        public event HC1Delegate HCDone;       

        //I funkcje sprawdzające Hard Constraints
        //konwencja zapisu HC+numner+Opis (HC - Hard Constraints)
        //funkcje powinny zwracac -1 gdy nie są spełnione Constraints lub 0 w przeciwnym wypadku

        public abstract bool HC1SchedulingPlanNeedsToBeFulfilled();
        #region 
        public abstract bool HC2EachDayOnlyOneShiftForNurse(); 
        public abstract bool HC3EachNurseCanExceedFourHourDuringSchedulingPeriod();
        public abstract bool HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod();
        public abstract bool HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod();
        public abstract bool HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired();
        public abstract bool HC7DuringPeriodOf24ConsecutiveHours11HoursOfRestIsRequired();
        public abstract bool HC8NightShiftMustBeFollowedByAtLeast14HoursOfRest();
        public abstract bool HC9NumberOfConsecutiveNightShiftsIsAtMost3();
        public abstract bool HC10NumberOfConsecutiveShiftsIsAtMost6();
        #endregion

        //II funkcje sprawdzające Soft Constraints - musza zwracac Penalty - z dokumentu Bargieły
        public abstract int SC2AvoidIsolatedWorkingDays();
        public abstract int SC4EmployeesOfAvability30HoursPerWeekLengthOfNightSeriesShouldBeWithinRange2To3();

        public abstract int SC5RestAfterSeriesOfDayEarlyLateShiftIsAMinimum2Days();

        public abstract int SC7EmployeesWithAvability30HoursPerWeekNumberOfShiftsIsBetween2Or3();

        public abstract int SC11LengthOfLateShiftsShouldBeBetween2Or3();

        public abstract int SC13NightShiftAfterEarlyShiftShouldBeAvoided();



        ///<summary> ///to jest opis co funkcja robi
        ///zwraca liczbe spełnionych constainrs i uruchamia zdarzenia dla spełnienia odpowiedniego constraints
        ///</summary>
        public virtual int checkHowMuchHardConstraintsIsDone()
        {
            int howMuchConstraintsDone = 0;
            bool ConstraintsFlag = true;   

            ConstraintsFlag = HC1SchedulingPlanNeedsToBeFulfilled();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            //pierwszys zawsze spełniony więc uruchamiam odpowidnie zdarzenia
            HCDone(1);
           
            ConstraintsFlag = HC2EachDayOnlyOneShiftForNurse();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            //gdy drugi spełniony to zdarzenie wywoływane
            HCDone(2);

            ConstraintsFlag = HC3EachNurseCanExceedFourHourDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(3);

            ConstraintsFlag = HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(4);

            ConstraintsFlag = HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(5);

            ConstraintsFlag = HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(6);

            ConstraintsFlag = HC7DuringPeriodOf24ConsecutiveHours11HoursOfRestIsRequired();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(7);

            ConstraintsFlag = HC8NightShiftMustBeFollowedByAtLeast14HoursOfRest();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(8);

            ConstraintsFlag = HC9NumberOfConsecutiveNightShiftsIsAtMost3();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            HCDone(9);

            ConstraintsFlag = HC10NumberOfConsecutiveShiftsIsAtMost6();
            HCDone(10);
            if (ConstraintsFlag == true) howMuchConstraintsDone++;


            return howMuchConstraintsDone;
        }

        /// <summary>
        /// zwraca kare za niespełnienie Soft Constraints
        /// </summary>
        public virtual int checkSoftConstraintsTemplateMethod()
        { 
            int penalty = 0;

            penalty += SC2AvoidIsolatedWorkingDays();

            penalty += SC4EmployeesOfAvability30HoursPerWeekLengthOfNightSeriesShouldBeWithinRange2To3();

            penalty += SC5RestAfterSeriesOfDayEarlyLateShiftIsAMinimum2Days();

            penalty += SC7EmployeesWithAvability30HoursPerWeekNumberOfShiftsIsBetween2Or3();

            penalty += SC11LengthOfLateShiftsShouldBeBetween2Or3();

            penalty += SC13NightShiftAfterEarlyShiftShouldBeAvoided();

            return penalty;
        }

        ///<summary> 
        ///zwraca penalty obecnego Chromosomu (kare za niespełnienie Soft Constraints)
        ///</summary>
        public virtual int checkConstraints(NurseClass[][][][] chromosomeVectorReference,PoolOfNurses obPoolOfNursesReference)
        {
			this.chromosomeVectorReference=chromosomeVectorReference; //uzupelniam referencje do Chromosoma
                                                                      //to trzeba przerzucic do konstruktora pozniej
            this.obPoolOfNursesReference = obPoolOfNursesReference;
			
            int howMuchHardConstraintsDone;
            int softConstraintPenalty = 0;

            howMuchHardConstraintsDone = checkHowMuchHardConstraintsIsDone();

           /* if(howMuchHardConstraintsDone<10)//jesli niespełnione hard constraints to już nie sprawdzam dalej
            {
                return -1;
            }*/

            softConstraintPenalty = checkSoftConstraintsTemplateMethod(); 
            return softConstraintPenalty;
        }

    }
}
