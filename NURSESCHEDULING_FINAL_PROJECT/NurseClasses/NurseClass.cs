using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NURSESCHEDULING_FINAL_PROJECT
{
    class NurseClass
    {
        public enum KindOfJob
        {
            parttime,
            fulltime
        }
        string name;
        string surname;
        sbyte idOfNurse;
        KindOfJob kindOfJob;  //full time czy part time
        public static sbyte counterOfNurseObject=1;

        public NurseClass(string name,string surname,KindOfJob kindOfJob)
        {
            this.name = name;
            this.surname = surname;
            this.idOfNurse = counterOfNurseObject;
            this.kindOfJob = kindOfJob;


            counterOfNurseObject++;   //kazda nowo utworzona Pielegniarka dostaje nową liczbe     
        }

        public sbyte ID
        {
            get
            {
                return idOfNurse;
            }
        }

        internal KindOfJob KindOfJob1 { get => kindOfJob; set => kindOfJob = value; }
    }
}
