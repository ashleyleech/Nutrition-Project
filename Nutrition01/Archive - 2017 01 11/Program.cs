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
            SimModel.loopThroughPolicies();



        }
    }
}
