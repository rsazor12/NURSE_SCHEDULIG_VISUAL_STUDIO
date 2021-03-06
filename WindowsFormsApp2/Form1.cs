﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NURSESCHEDULING_FINAL_PROJECT;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        bool buttonDown = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonDown = true;

            startProgram();
        }


        private int[] getFirstWeekArray()
        {
            //funkcja pobiera dane z pliku i zwraca int[],  ta tablica  dalej musi być przekaza  jako orgument 
            //  do funkcji initFirstWeek();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\Users\nikita";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            String line = "text" ;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.OpenFile()))
                    {
                        // Read the stream to a string, and write the string to the console.
                        line = sr.ReadToEnd();
                       // textBox1.Text = line;
                    }

                }
               
            }
            int[] firstWeekArr = line.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).
            Where(x => !string.IsNullOrWhiteSpace(x)).
            Select(x => int.Parse(x)).ToArray();

            return firstWeekArr;

        }


        public  void startProgram()
        {
            //NURSESCHEDULING_FINAL_PROJECT.Program.Main(getFirstWeekArray());

          


            //Console.SetBufferSize(200, 100);
            ChromosomeClass obChromosomeClass;// = new ChromosomeClass();
            //obChromosomeClass.writeNursesFromChromosomeFromEachShift();


            GeneticAlgorithmClass obGeneticAlgorithmClass = new GeneticAlgorithmClass(6, 1000, 20, 100, 1000, 4, getFirstWeekArray());
            obChromosomeClass = obGeneticAlgorithmClass.runAlgorithm();  //uruchom algorytm , a gdy wsytskie constraints spełnione zwróć najlepszy chromosom

            obChromosomeClass.writeNursesFromChromosomeFromEachShift();  //wypisz ten chromosom ktory jest wynikiem
            obChromosomeClass.exportChromosomeToExcel();

            Console.WriteLine("\n\nPenalty rozwiązania wynosi \a\a\a\a\a\a" + obChromosomeClass.PenaltyOfChromosome);
            Console.WriteLine("Poziom spełnienia HC : " + obChromosomeClass.checkHowManyHCIsDone());


            Console.ReadLine();
            Console.ReadLine();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

        }


        public async void refreshStatusBar()
        {
            int lastGeneration = -1;

            if (buttonDown == true)
            {
                if (GeneticAlgorithmClass.counterOfGenerations != lastGeneration)
                {
                    lastGeneration = GeneticAlgorithmClass.counterOfGenerations;
                    
                }

            }
        }
    }


}
