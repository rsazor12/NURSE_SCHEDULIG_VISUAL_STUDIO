using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NURSESCHEDULING_FINAL_PROJECT;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void HC1Test1()
        //zawsze zrwaca true, ponieważ w klasie init klasy Chromosome mamy od razu wypełniany chromosom
        //tak więc plan zawsze jest wypełniony
        //dodatkowo zawsze musimy mieć chromosom więc nie wykonuje drugiego testu
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            bool wynik = chromosom_testowy.obConstraintsClass.HC1SchedulingPlanNeedsToBeFulfilled();
            Assert.AreEqual(true, wynik);
        }


        [Test]
        public void HC2Test1()
        // test dla HC2- For each day a nurse may start only one shift.
        //Wynik powinien być negatywny- jedna pielęgniarka występuje w 3 zmianach podczas jednego dnia
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();

            NurseClass nowa_pielęgniarka = chromosom_testowy.obPoolOfNurses.getRandomNurseFromPool();
            //dodaję tą pielęgniarkę na 3 zmiany tego samego dnia w tym samym tygodniu
            chromosom_testowy.chromosomeVector[0][0][1][0] = nowa_pielęgniarka;
            chromosom_testowy.chromosomeVector[0][0][1][1] = nowa_pielęgniarka;
            chromosom_testowy.chromosomeVector[0][0][1][2] = nowa_pielęgniarka;

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            bool wynik = chromosom_testowy.obConstraintsClass.HC2EachDayOnlyOneShiftForNurse();

            Assert.AreEqual(false, wynik);
        }

        [Test]
        public void HC2Test2()
        // test dla HC2- For each day a nurse may start only one shift.
        //Wynik powinien być pozytywny
        //Idea testu- codziennie wykorzystujemy ten sam zestaw pielęgniarek- tworzymy tablicę 10 pielęgniarek i od
        //poniedziałku do piątku wykorzystujemy pięlęgniarki 1-10 a w weekendy pielęgniarki 1-7. 
        //Takim sposobem żadna z pielęgniarek nie powtórzy się dwukrotnie podczas jednego dnia.
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass[] NurseTab = new NurseClass[10];
            int week, day, shift;

            //bierzemu pierwsze 10 pielęgniarek z tablicy PoolOfNurses
            for (int i = 0; i < 10; i++)
            {
                NurseTab[i] = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex((sbyte)i);
            }

            //Każdego dnia na każdą zmianę przypisujemy ten sam skład pielęgniarek, które się nie powtarzają.
            week = day = shift = 0;
            for (week = 0; week < 5; week++)
            {
                for (day = 0; day < 7; day++)
                {
                    if (day < 5)
                    {
                        chromosom_testowy.chromosomeVector[week][day][0][0] = NurseTab[0];
                        chromosom_testowy.chromosomeVector[week][day][0][1] = NurseTab[1];
                        chromosom_testowy.chromosomeVector[week][day][0][2] = NurseTab[2];
                        chromosom_testowy.chromosomeVector[week][day][1][0] = NurseTab[3];
                        chromosom_testowy.chromosomeVector[week][day][1][1] = NurseTab[4];
                        chromosom_testowy.chromosomeVector[week][day][1][2] = NurseTab[5];
                        chromosom_testowy.chromosomeVector[week][day][2][0] = NurseTab[6];
                        chromosom_testowy.chromosomeVector[week][day][2][1] = NurseTab[7];
                        chromosom_testowy.chromosomeVector[week][day][2][2] = NurseTab[8];
                        chromosom_testowy.chromosomeVector[week][day][3][0] = NurseTab[9];
                    }
                    if (day > 4)
                    {
                        chromosom_testowy.chromosomeVector[week][day][0][0] = NurseTab[0];
                        chromosom_testowy.chromosomeVector[week][day][0][1] = NurseTab[1];
                        chromosom_testowy.chromosomeVector[week][day][1][0] = NurseTab[2];
                        chromosom_testowy.chromosomeVector[week][day][1][1] = NurseTab[3];
                        chromosom_testowy.chromosomeVector[week][day][2][0] = NurseTab[4];
                        chromosom_testowy.chromosomeVector[week][day][2][1] = NurseTab[5];
                        chromosom_testowy.chromosomeVector[week][day][3][0] = NurseTab[6];
                    }

                }
            }

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            bool wynik = chromosom_testowy.obConstraintsClass.HC2EachDayOnlyOneShiftForNurse();

            Assert.AreEqual(true, wynik);
        }

        [Test]
        public void HC3Test1()
        //Pielęgniarka o okresie planowania może przekroczyć liczbę godzin pracy o 4h
        //Idea testu- pielęgniarke pracującą full- time(36h) wpisujemy do grafiku przez
        //5 tygodni na 5 "ósemek" - wtedy przekroczy dopuszczalny czas pracy o 20 godzin a nie o 4
        //Oczekiwany wynik: false 
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(0);
            int week, day;
            week = day = 0;

            //sprawdzamy czy jest fulltime
            if ((nurse_tes.KindOfJob1) == NurseClass.KindOfJob.fulltime)
            {
                for (week = 0; week < 5; week++)
                {
                    for (day = 0; day < 5; day++)
                    {
                        chromosom_testowy.chromosomeVector[week][day][0][0] = nurse_tes;

                    }
                }
            }
            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC3EachNurseCanExceedFourHourDuringSchedulingPeriod();
            Assert.AreEqual(false, wynik);
        }

        [Test]
        public void HC4Test1()
        //The maximum number of	night shifts is	3 per period of	5 consecutive weeks.
        //Maksymalnie 3 nocne zmiany na 5 tygodni
        //Idea testu- Do chromosomu wprowadzamy tę samą pielęgniarkę na 5 nocnych zmian w ciągu 1 tygodnia
        //Oczekiwany wynik: false 
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);

            for (int day = 0; day < 5; day++)
            {
                chromosom_testowy.chromosomeVector[0][day][3][0] = nurse_tes;
                //1 tydzień, przez 5 dni na 3 zmianie ta sama pielęgniarka
            }

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod();

            Assert.AreEqual(false, wynik);
        }

        [Test]
        public void HC4Test2()
        //The maximum number of	night shifts is	3 per period of	5 consecutive weeks.
        //Maksymalnie 3 nocne zmiany na 5 tygodni
        //Idea testu-Każdą pielęgniarke wpisujemy na 3 nocne zmiany po kolei. Tym sposobem żadna nie będzie pracowała więcej
        //niż 3 noce. 
        //Oczekiwany wynik: true 
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass[] NurseTab = new NurseClass[16];
            int week, day, shift, i;
            int numer, licznik;

            for (i = 0; i < 16; i++)
            {
                NurseTab[i] = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex((sbyte)i);
            }

            week = day = shift = licznik = numer = 0;
            for (week = 0; week < 5; week++)
            {
                for (day = 0; day < 7; day++)
                {
                    chromosom_testowy.chromosomeVector[week][day][3][0] = NurseTab[numer];
                    ++licznik;
                    if (licznik == 3)
                    {
                        licznik = 0;
                        ++numer;
                    }
                }
            }


            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC4MaxThreeNightShiftForNurseDuringSchedulingPeriod();

            Assert.AreEqual(true, wynik);

        }

        [Test]
        public void HC5Test1()
        //•A nurse must receive at least 2 weekends off duty per 5 week period. A weekend off duty
        //lasts 60 hours including Saturday 00:00 to Monday 04:00.
        //Pielęgniarka musi mieć przynajmniej 2 weekendy wolne, po 60h, zawierające w tym czas od Soboty 00:00 do Ponidziałku 4:00
        //Idea testu-Wybraną pielęgniarkę wpisujemy do planu na 3 weekendy
        //Oczekiwany wynik: false 
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);

            int week, day;
            week = day = 0;

            //sprawdzamy czy jest fulltime
            if ((nurse_tes.KindOfJob1) == NurseClass.KindOfJob.fulltime)
            {
                for (week = 0; week < 3; week++)
                {
                    for (day = 5; day < 7; day++)
                    {
                        chromosom_testowy.chromosomeVector[week][day][3][0] = nurse_tes;
                    }
                }
            }

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod();

            Assert.AreEqual(false, wynik);

        }

        [Test]
        public void HC5Test2()
        //•A nurse must receive at least 2 weekends off duty per 5 week period. A weekend off duty
        //lasts 60 hours including Saturday 00:00 to Monday 04:00.
        //Pielęgniarka musi mieć przynajmniej 2 weekendy wolne, po 60h, zawierające w tym czas od Soboty 00:00 do Ponidziałku 4:00
        //Idea testu-Na każde nocne zmimany w weekendy wpisujemy inne pielęgniarki
        //Oczekiwany wynik: true 
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass[] NurseTab = new NurseClass[10];
            int week, day, shift, i;
            int numer;

            for (i = 0; i < 10; i++)
            {
                NurseTab[i] = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex((sbyte)i);
            }

            week = day = shift = numer = 0;
            for (week = 0; week < 5; week++)
            {
                for (day = 4; day < 7; day++)
          
                {
                    for (shift=0; shift < 3; shift++)

                    {
                        chromosom_testowy.chromosomeVector[week][day][shift][0] = NurseTab[numer];
                        chromosom_testowy.chromosomeVector[week][day][shift][1] = NurseTab[numer];
                        chromosom_testowy.chromosomeVector[week][day][shift][2] = NurseTab[numer];
                    }
                    shift = 3;
                    chromosom_testowy.chromosomeVector[week][day][shift][0] = NurseTab[numer];
                }
                ++numer;
            }


            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC5AtLeastTwoWeekendsOffDutyForNurseDuringSchedulingPeriod();

            Assert.AreEqual(true, wynik);
        }

        [Test]
        public void HC6Test1()
        //Following a series of at least 2 consecutive night shifts a 42 hours rest is required.
        //Po serii 2 nocek wymagany 2dniowy odpoczynek
        //Idea testu- wpisujemy wybraną pielęgniarkę na 3 dni pod rząd na nocną zmianę
        //Oczekiwany wynik: false
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);

            //3 dni pod rząd nocka
            chromosom_testowy.chromosomeVector[0][0][3][0] = nurse_tes;
            chromosom_testowy.chromosomeVector[0][1][3][0] = nurse_tes;
            chromosom_testowy.chromosomeVector[0][2][3][0] = nurse_tes;

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired();

            Assert.AreEqual(false, wynik);


        }

        [Test]
        public void HC6Test2()
        //Following a series of at least 2 consecutive night shifts a 42 hours rest is required.
        //Po serii 2 nocek wymagany 2dniowy odpoczynek
        //Idea testu- wpisujemy na zmianę 4 pielęgniarki na nocne zmiany
        //Oczekiwany wynik: true
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes1 = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);
            NurseClass nurse_tes2 = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(2);
            NurseClass nurse_tes3 = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(3);
            NurseClass nurse_tes4 = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(4);
            int week, day;
            week = day = 0;

            for (week = 0; week < 5; week++)
            {
                for (day = 0; day < 2; day++)
                {
                    chromosom_testowy.chromosomeVector[week][day][3][0] = nurse_tes1;
                }
                for (day = 2; day < 4; day++)
                {
                    chromosom_testowy.chromosomeVector[week][day][3][0] = nurse_tes2;
                }
                for (day = 4; day < 6; day++)
                {
                    chromosom_testowy.chromosomeVector[week][day][3][0] = nurse_tes3;
                }
                day = 6;
                {
                    chromosom_testowy.chromosomeVector[week][day][3][0] = nurse_tes4;
                }
            }

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC6AfterSeriesOfAtLeastTwoConsecutiveNights42HoursOfRestIsRequired();

            Assert.AreEqual(true, wynik);

        }

        [Test]
        public void HC7Test1()
        //During any period of 24 consecutive hours, at least 11 hours of rest is required.
        //W ciągu każdych 24 godzin,11 godzin odpoczynku jest wymagane
        //Idea testu- wybraną pielęgniarkę umieszczamy na zmiany early i night.
        //Oczekiwany wynik: false
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);


            chromosom_testowy.chromosomeVector[0][0][0][0] = nurse_tes;//zmiana early
            chromosom_testowy.chromosomeVector[0][0][3][0] = nurse_tes;//zmiana night

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC7DuringPeriodOf24ConsecutiveHours11HoursOfRestIsRequired();

            Assert.AreEqual(false, wynik);

        } //drugiego przypadku nie opracuje- zbyt skomplikowany

        [Test]
        public void HC8Test1()
        //A night shift has to be followed by at least 14 hours rest. An exception is that once in a
        //period of 21 days for 24 consecutive hours, the resting time may be reduced to 8 hours.
        //Nocna zmiana musi być poprzedzona 14godiznnym odpoczynkiem. Wyjątek: okres 21 dni gdzie 24h pracy, wtedy
        //czas odpocztynku może zostać zredukowany do 8h.
        //Oczekiwany wynik: true

        //*******************    TUTAJ NIE JESTEM PEWNA CZY TO ZAWSZE POWINNO ZWRACAC TRUE?      ********
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC8NightShiftMustBeFollowedByAtLeast14HoursOfRest();

            Assert.AreEqual(true, wynik);
        }

        [Test]
        public void HC9Test1()
        // Liczba kolejnych nocnych zmian wynosi co najwyżej 3.
        //jeden z poprzednich Constriants już to zapewnia(HC4)- to ograniczenie zawsze zwraca true
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC9NumberOfConsecutiveNightShiftsIsAtMost3();

            Assert.AreEqual(true, wynik);
        }

        [Test]
        public void HC10Test1()
        //The number of consecutive shifts (workdays) is at most 6.
        //Maksymalnie liczba zmian w tygodniu to 6.
        //Idea testu: wybraną pielęgniarkę zapisujemy do grafiku codziennie.
        //Oczekiwany wynik: false.
        {
            ChromosomeClass chromosom_testowy = new ChromosomeClass();
            NurseClass nurse_tes = chromosom_testowy.obPoolOfNurses.getNurseFromPoolFromTheIndex(1);

            int day = 0;

            for (day = 0; day < 7; day++)
                chromosom_testowy.chromosomeVector[0][day][0][0] = nurse_tes; //zapisujemy przez cały tydzień na zmiane early

            chromosom_testowy.obConstraintsClass.chromosomeVectorReference = chromosom_testowy.chromosomeVector;
            chromosom_testowy.obConstraintsClass.obPoolOfNursesReference = chromosom_testowy.obPoolOfNurses;
            bool wynik = chromosom_testowy.obConstraintsClass.HC10NumberOfConsecutiveShiftsIsAtMost6();

            Assert.AreEqual(false, wynik);

        }//drugiego testu nie zrobie - zbyt skomplikowany


    }
}
