using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrition01
{
    class SimModel
    {
        // Simulation cohort list pointer
        public static List<Person> Results;

        // List of statistics
        static LinkedList<string> outputStats;

        // String containing the simulation results;
        static String simOutputString = new string(' ', 0);

        // Discount rate
        static public double discountRatePerYr;
        static public double deltaTime;
        static public Boolean debugOn;
        static int Sim_NumSubjects;

        /// <summary>
        /// loopThroughPolicies - for each policy in the PolicyList, procedure
        /// extracts policy information, runs simulation, and reports results.
        /// </summary>
        static public void loopThroughPolicies()
        {
            // Static globals for simulation
            discountRatePerYr = Parms.getParmDouble("discountRate");
            deltaTime = Parms.getParmDouble("deltaTime");
            debugOn = Convert.ToBoolean(Parms.getParmDouble("DebugOn"));
            int Sim_NumSubjects = (int)Parms.getParmDouble("Sim_NumSubjects");
            setUpStatList();

            foreach (Policy myPolicy in Policy.PolicyList)
            {
                initResultsList();
                runSim(myPolicy);
                reportAllStats(myPolicy);
            }
        }


        /// <summary>
        /// runSimulation(myPolicy)
        /// Loops through Popoulation and runs simulation for each population member until
        /// the size of the simulated cohort reaches the user-specified sim_NumSubjects.
        /// </summary>
        static private void runSim(Policy myPolicy)
        {
            int popPersonNum; // Offset of person in the population array we are copying
            Person popPerson; // Points to the person in the population array we are copying
            Person simPerson; // Points to a simulated person (copied from population array)

            for (int i = 0; i < Sim_NumSubjects; i++)
            {
                popPersonNum = i % Person.Population.Count();
                popPerson = Person.Population[i % Person.Population.Count()];
                simPerson = new Person(popPerson);
                simPerson.simMyLife(myPolicy);
                Results.Add(simPerson); // Add copied person to resultList
                reportSimProgress(i); // Report progress
            }
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Reports out number of subjects simulated at specified interval.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        private static void reportSimProgress(int numSimulated)
        {
            if (numSimulated % Parms.getParmDouble("Sim_OutputInterval") == 0)
                Console.WriteLine("Simulated {0} of {1} subjects",
                    numSimulated, Parms.getParmDouble("Sim_NumSubjects"));
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// reportAllStats
        /// Reports out simulation results for myPolicy.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static public void reportAllStats(Policy myPolicy)
        {
            simOutputString = "";

            simOutputString += "Policy: " + myPolicy.policyName;

            // Set up linked list to hold the values from one quantity for which we will
            // compute summary statistics.
            List<Double> tempValues = new List<Double>();

            // Get summary stats for life years.
            tempValues.Clear();
            foreach (Person myPerson in Results) { tempValues.Add(myPerson.LY); }
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";

        }

        static String reportStatsOneQuantity(List<Double> values)
        {
            String returnString = "";
            double resultHolder;
            double mean = 0;
            values.Sort();

            // Compute mean - may be used for computation of variance
            double sum = 0;
            foreach (double d in values) { sum += d; }
            mean = sum / values.Count();

            foreach (string stat in outputStats)
            {

                // Output mean if it that stat is requested
                if (stat.Equals("mean"))
                {
                    returnString = returnString + "mean:\t" + mean.ToString() + "\n";
                }

                // Output variance if that stat is requested
                if (stat.Equals("variance"))
                {
                    sum = 0;
                    foreach (double d in values) { sum += (mean - d) * (mean - d); }
                    resultHolder = sum / values.Count();
                    returnString = returnString + "variance:\t" + resultHolder.ToString() + "\n";
                }

                // Output min and max if they are requested
                if (stat.Equals("min"))
                {
                    resultHolder = values[0];
                    returnString = returnString + "minimum:\t" + resultHolder.ToString() + "\n";
                }
                if (stat.Equals("max"))
                {
                    resultHolder = values[values.Count() - 1];
                    returnString = returnString + "maximum:\t" + resultHolder.ToString() + "\n";
                }

            }
            System.Console.WriteLine(returnString);
            return returnString;
        }


        //****************************************************************************************
        // Creates a linked list of statistics to report out for each outcome variable
        //****************************************************************************************
        static void setUpStatList()
        {
            outputStats = new LinkedList<String>();
            outputStats.AddLast("mean");
            outputStats.AddLast("variance");
            outputStats.AddLast("min");
            outputStats.AddLast("max");
            outputStats.AddLast("pctl|0.05");
        }

        /// <summary>
        /// Clean up Results list and reinitialize it.
        /// </summary>
        static void initResultsList()
        {
            Results = null;
            GC.Collect();
            Results = new List<Person>();
        }

    }
}
