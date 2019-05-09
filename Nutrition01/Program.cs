using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrition01
{
    class Program
    {
        static void Main(string[] args)
        {

            if (!Nutrition01.SimIO.InitializeIO(args)) Environment.Exit(0);
            //Person myPerson = new Person();
            //myPerson.myTest();

            SimModel.loopThroughPolicies();




            /*           
            Person myPerson = new Person();
            myPerson.myTest();

             */


        }
    }
}
