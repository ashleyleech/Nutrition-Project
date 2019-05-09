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

        // Parameters
        static public double deltaTime;
        static public double discountRatePerYr;
        static public Boolean debugOn;
        static public int Sim_NumSubjects;
        static public int StartYear;
        public static double maxAge;
        public static double maxSimDuration;

        // DEBUG xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    //    static public int[] diseaseArray = new int[8];

        /// <summary>
        /// loopThroughPolicies - for each policy in the PolicyList, procedure
        /// extracts policy information, runs simulation, and reports results.
        /// </summary>
        static public void loopThroughPolicies()
        {
            // Static globals for simulation
            deltaTime = Parms.getParmDouble("deltaTime");
            discountRatePerYr = Parms.getParmDouble("discountRate");
            debugOn = Convert.ToBoolean(Parms.getParmDouble("DebugOn"));
            Sim_NumSubjects = (int)Parms.getParmDouble("Sim_NumSubjects");
            StartYear = (int)Parms.getParmDouble("StartYear");
            maxAge = Parms.getParmDouble("MaxAge");
            maxSimDuration = Parms.getParmDouble("MaxSimDuration");

            setUpStatList();

            foreach (Policy myPolicy in Policy.PolicyList)
            {
                initResultsList();
                runSim(myPolicy);
                reportAllStats(myPolicy);
            }
        }


        /// <summary>
        /// runSim(myPolicy)
        /// Loops through Popoulation and runs simulation for each population member until
        /// the size of the simulated cohort reaches the user-specified sim_NumSubjects.
        /// </summary>
        static public void runSim(Policy myPolicy)
        {
            int popPersonNum; // Offset of person in the population array we are copying
            Person popPerson; // Points to the person in the population array we are copying
            Person simPerson; // Points to a simulated person (copied from population array)

            // DEBUG xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
         /*   for (int i = 1; i <= 7; i++)
            {
                diseaseArray[i] = 0;
            }*/


            for (int i = 0; i < Sim_NumSubjects; i++)
            {
                popPersonNum = i % Person.Population.Count();
                popPerson = Person.Population[i % Person.Population.Count()];
                simPerson = new Person(popPerson);
                simPerson.simMyLife(myPolicy);
                Results.Add(simPerson); // Add copied person to resultList
                reportSimProgress(i); // Report progress
            }
            reportAllStats(myPolicy);
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

            simOutputString += "Policy: " + myPolicy.policyName + "\n";
            simOutputString += "Country: " + myPolicy.country + "\n";

            // Set up linked list to hold the values from one quantity for which we will
            // compute summary statistics.
            List<Double> tempValues = new List<Double>();

            // Get summary stats for life years.
            tempValues.Clear();
            foreach (Person myPerson in Results) { tempValues.Add(myPerson.LY); }
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            // Get summary stats for "ever" disease.
            foreach (Person myPerson in Results) { tempValues.Add(myPerson.everStunting); }
            simOutputString = simOutputString + "EverStunted: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.everAnemia); }
            simOutputString = simOutputString + "EverAnemic: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.everDiarrhea); }
            simOutputString = simOutputString + "EverDiarrhea: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.isAlive_0); }
            simOutputString = simOutputString + "Alive @ age 0: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.Stunting_0); }
            simOutputString = simOutputString + "Stunting @ age 0: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.Anemia_0); }
            simOutputString = simOutputString + "Anemia @ age 0: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            foreach (Person myPerson in Results) { tempValues.Add(myPerson.Diarrhea_0); }
            simOutputString = simOutputString + "Diarrhea @ age 0: \n";
            simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            System.Console.WriteLine(simOutputString);

            //foreach (Person myPerson in Results) { tempValues.Add(myPerson.isAlive_0); }
            //simOutputString = simOutputString + "Alive @ age 5: \n";
            //simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            //System.Console.WriteLine(simOutputString);

            //foreach (Person myPerson in Results) { tempValues.Add(myPerson.Stunting_5); }
            //simOutputString = simOutputString + "Stunting @ age 5: \n";
            //simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            //System.Console.WriteLine(simOutputString);

            //foreach (Person myPerson in Results) { tempValues.Add(myPerson.Anemia_5); }
            //simOutputString = simOutputString + "Anemia @ age 5: \n";
            //simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            //System.Console.WriteLine(simOutputString);

            //foreach (Person myPerson in Results) { tempValues.Add(myPerson.Diarrhea_5); }
            //simOutputString = simOutputString + "Diarrhea @ age 5: \n";
            //simOutputString = simOutputString + reportStatsOneQuantity(tempValues) + "\n";
            //System.Console.WriteLine(simOutputString);


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
                    //resultHolder = values[0];
                    //returnString = returnString + "minimum:\t" + resultHolder.ToString() + "\n";
                }
                if (stat.Equals("max"))
                {
                    //resultHolder = values[values.Count() - 1];
                    //returnString = returnString + "maximum:\t" + resultHolder.ToString() + "\n";
                }

            }
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
