using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class ConcreteConstraintsClass : AbstractConstraintsClass
    {

       
        public ConcreteConstraintsClass()
        {
            
        }

        public override bool HC1SchedulingPlanNeedsToBeFulfilled()//to w init klasy chromosome mamy zapewnione 
        {
            return true;
        }

        //musze wziac kazda pielegniarke z dnia i sprawdzic czy pracuje one jeszcze w tym samym dniu
        public override bool HC2EachDayOnlyOneShiftForNurse()
		{
            NurseNavigator.clearChromosomeStatements(); //zeruje zeby pobierało od początku Chromosoma te pielegniarki
			NurseClass checkedNurse= NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference);
			
            while(checkedNurse!=null)
            {
                if(CheckThatIsOnlyOneNurseOnDay(checkedNurse, NurseNavigator.CurrentWeek, NurseNavigator.CurrentDay)==false)//jesli zwroci false to pielegniarki sie powatrzaja w danym dniu wiec przerywam sprawdzanie
                {
                    return false;
                }
                checkedNurse = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference);
            }             		
            return true;
		}
        /// <summary>
        /// Zwraca true jesli jest tylko jedna pielegniarka w danym dniu
        /// </summary>
        private bool CheckThatIsOnlyOneNurseOnDay(NurseClass checkedNurse, sbyte week, sbyte day)
        {
            //tu musze sprawdzic czy jest tylko jedna w danym dniu
            int duplicateNursesCounter=0; //pielegniarki ktore sie powatrzaja

            //wybieram wszystkie pielegnairki z danego dnia i sprawdzam z podana w paramatrze
           
            for (sbyte shift = 0; shift < 4; shift++)
            {
                for (int nurse = 0; nurse < chromosomeVectorReference[week][day][shift].Length; nurse++)
                    if (chromosomeVectorReference[week][day][shift][nurse].ID == checkedNurse.ID)
                        duplicateNursesCounter++;     
             }

            if (duplicateNursesCounter > 1) return false;  //jesli wiecej niz jedna
            return true;                                   //jesli jedna
        }

        /// <summary>
        /// true jesli spelniony
        /// </summary>
        public override bool HC3EachNurseCanExceedFourHourDuringSchedulingPeriod()
        {
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;


            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();

            int howManyHoursNurseWorkInPlan = 0;

            while((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference))!=null)//dla kazdej pielegniarki z pool
            {
                while((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference))!=null)
                {
                    if(checkedNurseFromPool==checkedNurseFromChromosome)
                    {
                        howManyHoursNurseWorkInPlan = howManyHoursNurseWorkInPlan + (int)ShiftNavigator.getCurrentShiftLengthForNurseFromNurseNavigator();
                    }
                }

                //dla kazdej pielegniarki teraz sprawdzam czy ilosc wystapien * ilosc_godzin_pracy +4  jest mniejsza niz ilosc mozliwych godzin pracy przez cały plan
                if(checkedNurseFromPool.KindOfJob1==NurseClass.KindOfJob.fulltime) //jesli sprawdzana była fulltime
                {
                    //jezeli pracowala dluzej niz 36*5+4 to zwracam false jesli nie to sprawdzam dalej
                    if(howManyHoursNurseWorkInPlan >= (36*5+4))
                    {
                        return false;
                    }
                }
                else //jesli part time
                {
                    if (howManyHoursNurseWorkInPlan >= (20 * 5 + 4))
                    {
                        return false;
                    }
                }

                howManyHoursNurseWorkInPlan = 0;
                NurseNavigator.clearChromosomeStatements();
                //NurseNavigator.clearPoolStatements();
            }

            return true;  //jesli sprawdzanie bez zakłucen        
        }
        /// <summary>
        /// true jesli spelniony
        /// </summary>
        public override bool HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod()
        {
            int numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod = 0;
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;

            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    if (checkedNurseFromPool == checkedNurseFromChromosome)
                    {
                        //if (ShiftNavigator.CurrentKindOfShift == ShiftNavigator.KindOfShift.night) //zawsze early
                        if (NurseNavigator.CurrentShift == 3)
                        {
                            numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod++;

                            if (numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod > 3) return false;
                        }

                    }
                }

                numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod = 0;
                NurseNavigator.clearChromosomeStatements();

            }

            // NurseNavigator.clearPoolStatements();

            return true;

        }

        public override bool HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod()
        {
            //pobieram pielegniarki z odpowiedniego przedziału (przeziałem jest weekend - ma byc 60 godzin - dlatego taki długi
            NurseNavigator.setIntervalForReturnedNursesFromChromosome(0,4,(sbyte)ShiftNavigator.DaysOfWeek.Friday,(sbyte)ShiftNavigator.DaysOfWeek.Sunday,0,3);

            int numberOfWeekendsOfCurrentNurseDuringSchedulingPeriod = 0;
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;

            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    if (checkedNurseFromPool == checkedNurseFromChromosome)
                    {
                        if (ShiftNavigator.CurrentKindOfShift == ShiftNavigator.KindOfShift.night)
                        {
                            numberOfWeekendsOfCurrentNurseDuringSchedulingPeriod++;

                            if (numberOfWeekendsOfCurrentNurseDuringSchedulingPeriod == 2) break;  //jesli 2 weekendy wolne to nie sprawdzaj dalej
                        }
                    }
                }

                if (numberOfWeekendsOfCurrentNurseDuringSchedulingPeriod < 2) return false; //ta pielegniarka nie miała 2 weekendów wolnych więc cały constraints niespełniony

                numberOfWeekendsOfCurrentNurseDuringSchedulingPeriod = 0;  //dla nowej pielegniarki zerujemy licznik
                NurseNavigator.clearChromosomeStatements();  //od nowa pobieranie z chromosoma

            }

           // NurseNavigator.clearPoolStatements();

            return true;  //nie przerwano wiec constraints spełniony
        }

        public override bool HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired()
        {
            int numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod = 0;
            int numberOfNotNightShiftAfterNightShift = 0; //jesli przynajmniej 8 to constraints spełniony

            int numberOfLastNightShift = 0;
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;

            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    if (checkedNurseFromPool == checkedNurseFromChromosome)
                    {
                        if (ShiftNavigator.CurrentKindOfShift == ShiftNavigator.KindOfShift.night)
                        {
                            numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod++;
                        }
                        else //jesli nie nocka i nocek wiecej niz 2 to zaczynam liczyc zmiany wolne od nocek
                        {
                            if (numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod > 2)
                            {
                                //jezeli liczba nocek sie zmieni to zmienna przechowujaca zmiany wolne musi zostac wyzerowana
                                if (numberOfLastNightShift == numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod)
                                {
                                    numberOfNotNightShiftAfterNightShift++;
                                }
                                else
                                    numberOfNotNightShiftAfterNightShift = 0; //zeruje liczbe zmian wolnych bo mamy następną nocke
                               
                                numberOfLastNightShift = numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod; //to do kontroli czy liczba nocek sie zmieniła
                            }
                           
                        }
                    }
                    
                }
                if (numberOfNotNightShiftAfterNightShift < 8) return false; //jesli mniej niz 8 nastepnych zmian wolnych dla danej pielegniarku to nie odpoczywała wystarczajaco długo (42 h)
               
               
                //wyzerowanie dla nastęnej pielegniarku z PoolOfNurses
                numberOfNotNightShiftAfterNightShift = 0;
                numberOfLastNightShift = 0;
                numberOfNightShiftsOfCurrentNurseDuringSchedulingPeriod = 0;
                NurseNavigator.clearChromosomeStatements();
               // NurseNavigator.clearPoolStatements();
            }

            return true;
        }

        public override bool HC7DuringPeriodOf24ConsecutiveHours11HoursOfRestIsRequired()
        {
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;
            NurseClass checkedNurseFromNextShift;


            int howManyTimesNurseOccureIn3Shifts = 0;

            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();



            //zmienne do nawigacji po chromosomie - dzieki nimi bedziemy wracac 3 zmiany w tył w navigacji po chromosomie
            int weekOfCheckedShift;
            int dayOfCheckedShift;
            ShiftNavigator.KindOfShift checkedShift;
            int lastCheckedShift = 0;

            int counterOfCheckedShift = 0;

            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma(z wszystkich zmian po kolei)
                {
                    if (checkedNurseFromPool == checkedNurseFromChromosome)
                    {
                        //sprawdzamy czy dana pielegniarka występuje w przeciągu 4 zmian więcej niż 1 raz - jeśli tak to Constraints niespełniony

                        //do tego momentu w Chromosomie będę wracał jak sprawdze 3 następne zmiany
                        checkedShift = ShiftNavigator.CurrentKindOfShift; //przypisanie aktualnie sprawdzanej zmiany
                        dayOfCheckedShift = NurseNavigator.CurrentDay;
                        weekOfCheckedShift = NurseNavigator.CurrentWeek;

                        while (counterOfCheckedShift < 3) //jesli sprawdzono mniej niz 3 nastepne zmiany to sprawdzaj dalej
                        {
                            checkedNurseFromNextShift = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference);  //pobierz nastepną z tych zmian

                            if (lastCheckedShift != NurseNavigator.CurrentShift)//jesli pobieramy pielegniarki dalej z tej samej zmiany
                            {
                                counterOfCheckedShift++; //to znaczy ze pobrano juz wszystkie pielegniarki z danej zmiany
                            }
                            if (checkedNurseFromNextShift == checkedNurseFromPool)//jezeli jest na tych zmianach przymajmniej 2 razy to constraints niespełniony
                            {
                                howManyTimesNurseOccureIn3Shifts++;

                                if (howManyTimesNurseOccureIn3Shifts > 0) //jesli występuje w tych 3 nastepnych zmianach wiecej niz raz to Constraints niespełniony
                                {
                                    return false;
                                }
                            }
                            lastCheckedShift = NurseNavigator.CurrentShift;
                        }

                        //wracamy do tamtego miejsca w chromosomie gdzie zaczeliśmy sprawdzanie 3 nastepnych zmian
                        NurseNavigator.CurrentShift = (sbyte)(checkedShift + 1); //ale zmiane zmieniamy na o jeden wieksza
                        NurseNavigator.CurrentDay = (sbyte)dayOfCheckedShift;
                        NurseNavigator.CurrentWeek = (sbyte)weekOfCheckedShift;
                    }
                }

                NurseNavigator.clearChromosomeStatements();
            }
            return false; //jezeli program nie został przerwany w while to Constraint spełniony
        }

        public override bool HC8NightShiftMustBeFollowedByAtLeast14HoursOfRest() //to rowniez zawsze spełnione ponieważ w danym dniu można zacząc tylko jedną zmiane(HC1) - jesli zaczynam nocke to przeciez early,day,late mam wolne wczesniej
        {
            return true;
        }

        public override bool HC9NumberOfConsecutiveNightShiftsIsAtMost3() //jeden z poprzednich Constriants już to zapewnia(HC4)
        {
            return true;
        }

        public override bool HC10NumberOfConsecutiveShiftsIsAtMost6()
        {
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;

            int howManyTimesNurseOccureInConsecutiveDays = 0;   

            //zeruje odpowiednie indexy pobierania pielegniarek z navigatora - zeby pobierało od poczatku
            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();

            int lastCheckedDay = NurseNavigator.CurrentDay;


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma(z wszystkich zmian po kolei)
                {
                    if (checkedNurseFromPool == checkedNurseFromChromosome) //jeżeli to sprawdzana pielegniarka
                    {
                        if (lastCheckedDay != NurseNavigator.CurrentDay && ((lastCheckedDay + 1) == NurseNavigator.CurrentDay))//jesli sprawdzanie odbywa sie już dla innego dnia i jest to kolejny dzien
                        {
                            howManyTimesNurseOccureInConsecutiveDays++;  //podbijam wystąpienie pielegniarki w danym dniu

                            if (howManyTimesNurseOccureInConsecutiveDays == 6) return false; //jesli ten licznik wybije do 6 to cały constraints niespełniony
                        }
                        else   //jesli nie jest to kolejny dzien a pielegniarka wystąpiła to zeruje zmienna
                            howManyTimesNurseOccureInConsecutiveDays = 0;

                    }

                    lastCheckedDay = NurseNavigator.CurrentDay;
                }

                NurseNavigator.clearChromosomeStatements();
            }
            return false; //jezeli program nie został przerwany w while to Constraint spełniony
        }


        public override int SC2AvoidIsolatedWorkingDays() 
        {
            //Należy unikać izolowanych dni pracy - karą jest 1000 za każdy week kiedy są izolowane dni pracy , jest to np układ dni 0101110 - ten 2 dzien jest izolated
            //działanie algorytmu - zliczam dla każdej pielęgniarki ile ma takich izolowanych dni 

            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;
            int counterOfConsecutiveDaysForNurse=0;  //jesli 1 i nastepny dzien pusty to kara za cały week
            int lastCheckedDay = 0;
            int checkedDay = 0;
            int penalty = 0;

            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    checkedDay = NurseNavigator.CurrentDay;  //żeby wiedział który dzien jest obecnie sprawdzany

                    if(checkedNurseFromPool==checkedNurseFromChromosome)
                    {
                        if(checkedDay==lastCheckedDay-1) //jezeli dzień po dniu pielegniarka pracuje to zwiększam counter
                        {
                            counterOfConsecutiveDaysForNurse++;
                        }
                        else
                        {
                            //jeżeli nie pracuje dzien po dniu i counter==1 to mamy dzien izolowany
                            if (counterOfConsecutiveDaysForNurse == 1)
                            {
                                penalty += 1000; // dodaje penalty dla teko tygodnia i juz nie sprawdzam w tym tygodniu 

                                //przechodze do nastepnego tygodnia ze sprawdzaniem
                                NurseNavigator.setIntervalForReturnedNursesFromChromosome(NurseNavigator.CurrentWeek, 4, 0, 6, 0, 3);
                                counterOfConsecutiveDaysForNurse = 0;  //zeruje counter nastepujacych po sobie dni pracy
                                checkedDay = 0;
                                lastCheckedDay = 0;
                                
                            }
                                
                        }
                        lastCheckedDay = NurseNavigator.CurrentDay; //zapamiętuje ostatnio sprawdzony dzien z chromosoma
                    }
                }                  
            }
            return penalty;

        }




        public override int SC4EmployeesOfAvability30HoursPerWeekLengthOfNightSeriesShouldBeWithinRange2To3()
        {
            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;
            int lenghtOfSeries = 0;  
            int lastCheckedDay = 0;
            int checkedDay = 0;
            int penalty = 0;

            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    if(ShiftNavigator.CurrentKindOfShift == ShiftNavigator.KindOfShift.night)//jeżeli obecna zmiana z chromosoma to night
                    {
                        checkedDay = NurseNavigator.CurrentDay; //przypisuje dzien obecny w chromosomie

                        if(checkedDay==lastCheckedDay-1)//jezeli night shift w dniu po sobie
                        {
                            lenghtOfSeries++;
                        }
                        else //jeśli przerwaliśmy serie dni ze zmiana night
                        {
                            if ((lenghtOfSeries != 2) && (lenghtOfSeries != 3)) //jesli dlugosc serii to nie 2 ani nie 3
                                penalty = Math.Abs(lenghtOfSeries - 3) * 1000; // kara to różnica pomiędzy dlugoscia serii * 1000

                            lenghtOfSeries = 0;   //zerujemy licznik
                        }

                        lastCheckedDay = NurseNavigator.CurrentDay;
                        
                    }
                }
            }
            return penalty;
        }

        public override int SC5RestAfterSeriesOfDayEarlyLateShiftIsAMinimum2Days()
        {

            return 0;
        }

        public override int SC7EmployeesWithAvability30HoursPerWeekNumberOfShiftsIsBetween2Or3()
        //Dla pracowników którzy są dostępni 0-30h podczas tygodnia, liczba zmian w ciągu tygodnia powinna wynosić 2-3.
        //Sprawdzam dla każdej pielęgniarki part time ile ma zmian tygodniowo, jak więcej niż 3 to zwracam pentaly=10.
        {

            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;
            int weekOfCheckedShift;
            int numberOfShiftsOfCurrentNurseForCurrentWeek;
            int pentaly = 0;

            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.clearPoolStatements();

            weekOfCheckedShift = NurseNavigator.CurrentWeek; //pierwszy tydzień

            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                numberOfShiftsOfCurrentNurseForCurrentWeek = 0; //zeruje dla kolejnej pielęgniarki
                if (checkedNurseFromPool.KindOfJob1 != NurseClass.KindOfJob.parttime) continue; //jeżeli nie jest part time to pobieram kolejną z poolofnurses
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {
                    //tutaj musze sprawdzić czy przypadkiem tydzień się nie zmienił
                    //jeżeli tak to trzeba wyzerować numberOfShiftsOfCurrentNurseForCurrentWeek i dodać 1 do numeru tygodnia
                    if (weekOfCheckedShift < NurseNavigator.CurrentWeek)
                    {
                        weekOfCheckedShift++;
                        numberOfShiftsOfCurrentNurseForCurrentWeek = 0;
                    }

                    if (checkedNurseFromPool == checkedNurseFromChromosome) //jeżeli znajdzie taką pielęgniarkę
                    {
                        numberOfShiftsOfCurrentNurseForCurrentWeek++;

                        if (numberOfShiftsOfCurrentNurseForCurrentWeek > 3) pentaly += 10;//dodaje 10 jak któraś z pielęgniarek ma więcej zmian niż 3
                        //(***tutaj nie wiem czy powinno być 10 kary za to że to występuje czy 10 kary za każdą pielęgniarkę ale to łatwo przerobić***)

                    }
                }
                if (numberOfShiftsOfCurrentNurseForCurrentWeek < 2) pentaly += 10; //zwraca 10 jak któraś z pielęgniarek ma mniej niż 2 zmiany
            }

            //jak znalazło kare to zwraca kare a jak nie no to 0
            if (pentaly > 0)
                return pentaly;
            else
                return 0;
        }

        public override int SC11LengthOfLateShiftsShouldBeBetween2Or3()
        //Dla wszystkich pracowników liczba serii zmian late(wieczornych) powinna być w zakresie 2-3.
        //Ta seria zmian late może być częścią innej serii (czyli może być np. DDLLRRR)- albo przed nią albo po niej mogą
        //być jakieś inne zmiany. ** Ja to tak rozumiem. ** Ale tej drugiej części z serią chyba sie tutaj nie uwzględnia po prostu.
        {


            NurseClass checkedNurseFromPool;
            NurseClass checkedNurseFromChromosome;
            int numberOfLateShiftInTheSeries;
            int pentaly = 0;
            int remember_day = 0;
            //czyszcze pulę pielęgniarek- żeby je brało od początku
            NurseNavigator.clearPoolStatements();

            //ustawiam nawigator dla chromosomu żeby brało przez 5 tygodni tylko zmiany late
            NurseNavigator.setIntervalForReturnedNursesFromChromosome(0, 4, (sbyte)ShiftNavigator.DaysOfWeek.Monday, (sbyte)ShiftNavigator.DaysOfWeek.Sunday, 2, 2);


            while ((checkedNurseFromPool = NurseNavigator.getNextNurseFromNursePool(obPoolOfNursesReference)) != null)//dla kazdej pielegniarki z pool
            {
                numberOfLateShiftInTheSeries = 0; //zeruje dla kolejnej pielęgniarki   
                while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
                {

                    if (checkedNurseFromPool == checkedNurseFromChromosome)//jeżeli trafi na taką pielęgniarke w late
                    {
                        numberOfLateShiftInTheSeries++;
                        //jezeli to pierwsze wystąpienie to zapamiętuje sobie dzień, dzięki temu za dwa dni moge sprawdzić czy liczba 
                        //nocnych zmian od tamtej pory wzrosła (czyli czy nie było przerwy)- np. w poniedziałek zapamiętuje że pielęgniarka była na late,
                        //a potem w środe sprawdzam czy liczba jej late shift'ów to 2 czy nadal 1, bo jeżeli 1 to była w poniedziałek, we wtorek nie i w środe sprawdzam
                        if (numberOfLateShiftInTheSeries == 1)
                        {
                            remember_day = NurseNavigator.CurrentDay;
                        }

                        else if (NurseNavigator.CurrentDay == (remember_day + 2))
                        {
                            if (numberOfLateShiftInTheSeries == 2) pentaly += 10;
                            numberOfLateShiftInTheSeries = 1;
                        }
                        else if (numberOfLateShiftInTheSeries > 3)
                        //*** TUTAJ SIE TRZEBA ZASTANOWI CZY PENTALY SIE LICZY ZA KAŻDĄ ZA DUŻĄ SERIE ZMIAN NP. ***
                        //4 dni ze zmianą  + 10
                        //piąty dzień ze zmianą to kolejne + 10
                        //, czy np za każde 5 dni +10 i tyle. 
                        {
                            pentaly += 10;
                            numberOfLateShiftInTheSeries = 0;
                        }
                    }

                }
            }

            //jak znalazło kare to zwraca kare a jak nie no to 0
            if (pentaly > 0)
                return pentaly;
            else
                return 0;
        }


        public override int SC13NightShiftAfterEarlyShiftShouldBeAvoided()
        {
            // A night shift after an early shift should be avoided.
            // Nocna zmiana po zmianie early powinna być unikana. Pentaly = 1;
            // Sprawdzam każdego dnia dla każdej pielęgniarki ze zmiany early czy występuje w nocnej zmianie. Tego samego dnia?- Tak zakładam.
            // Jeśli tak to zwracam penaly. Jeżeli taka sytuacja nie zachodzi zwracam 0.
            //** Nie wiem czy to by było takie proste ale w sumie jeżeli sie bierze pod uwage ze nigdt nie moze byc po early tego samego dnia tylko no to na to wychodzi**

            NurseNavigator.clearChromosomeStatements();
            NurseNavigator.setIntervalForReturnedNursesFromChromosome(0, 4, (sbyte)ShiftNavigator.DaysOfWeek.Monday, (sbyte)ShiftNavigator.DaysOfWeek.Sunday, 0, 0);//wybieram pielęgniarki tylko z early

            NurseClass checkedNurseFromChromosome;
            int pentaly = 0;

            while ((checkedNurseFromChromosome = NurseNavigator.getNextNurseFromChromosome(chromosomeVectorReference)) != null) // dla kazdej z chromosoma
            {
                if (chromosomeVectorReference[NurseNavigator.CurrentWeek][NurseNavigator.CurrentDay][3][0] == checkedNurseFromChromosome) pentaly += 1;
            }


            if (pentaly > 0)
                return pentaly;
            else
                return 0;


        }
    }
}
