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

        //zdarzenia które będą powiadamiać o spełnieniu odpowiednich Constraints i ich niespełnieniu
        public delegate void HC1Delegate(int whichConstraintDone);
        public event HC1Delegate HCDone;

        public event HC1Delegate HCNotDone;

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
        ///zwraca liczbe spełnionych constaints i uruchamia zdarzenia dla spełnienia i niespełnia odpowiednich constraints
        ///</summary>
        public virtual int checkHowMuchHardConstraintsIsDone()
        {
            int howMuchConstraintsDone = 0;
            bool ConstraintsFlag = true;   

            //HC1
            ConstraintsFlag = HC1SchedulingPlanNeedsToBeFulfilled();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(1, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne
               
            //HC2        
            ConstraintsFlag = HC2EachDayOnlyOneShiftForNurse();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(2, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC3
            ConstraintsFlag = HC3EachNurseCanExceedFourHourDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(3, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC4
            ConstraintsFlag = HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(4, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC5
            ConstraintsFlag = HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(5, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC6
            ConstraintsFlag = HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(6, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC7
            ConstraintsFlag = HC7DuringPeriodOf24ConsecutiveHours11HoursOfRestIsRequired();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(7, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC8
            ConstraintsFlag = HC8NightShiftMustBeFollowedByAtLeast14HoursOfRest();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(8, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC9
            ConstraintsFlag = HC9NumberOfConsecutiveNightShiftsIsAtMost3();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(9, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            //HC10
            ConstraintsFlag = HC10NumberOfConsecutiveShiftsIsAtMost6();
            if (ConstraintsFlag == true) howMuchConstraintsDone++;
            executeEventForConstraint(10, ConstraintsFlag); // wywoła zdarzenie HCDone albo HCNotDOne

            return howMuchConstraintsDone;
        }

        private void executeEventForConstraint(int whichConstraint, bool constraintsFlag)
        {
          
            if (constraintsFlag == true)
            {                
                //pierwszys zawsze spełniony więc uruchamiam odpowiednie zdarzenia
                HCDone(1);
            }
            else
                HCNotDone(1); //zdarzenie bedzie powiadamiac o tym ze HC niespełnione
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
