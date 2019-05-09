using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRandom;
using System.Text.RegularExpressions;

namespace Nutrition01
{
    public class Person
    {
        // Delimiter
        static String fieldDelimiter = @","; // Delimiter for elements within a list

        // Lists
        public static List<Person> Population = new List<Person>();

        // Fields - initialization values
        private bool initializeOK;
        private double ageStart;
        private bool haveDiarrheaStart;
        private bool haveAnemiaStart;
        private bool haveStuntingStart;

        // Status fields
        private double ageNow;
        private bool haveDiarrheaNow;
        private bool haveAnemiaNow;
        private bool haveStuntingNow;
        private bool alive;

        // Outcome fields
        public double LY;
        public double LYDisc;
        public double QALYs;
        public double costs;

        // Valid field values
        public enum GenderType { FEMALE, MALE }
        public GenderType gender;
        public static Dictionary<string, int> genderTypeList = new Dictionary<string, int>()
        {
            {"FEMALE",0 },
            {"MALE",1 },
        };

        // Field locations in ititialization data vector
        static int gender_offset = 0;
        static int ageStart_offset = 1;
        public static int haveDiarrhea_offset = 2;
        public static int haveAnemia_offset = 3;
        public static int haveStunting_offset = 4;
        static int numPersonFields = 5;

        // Constructor
        public Person()
        {
            initializeOK = true;
        }

        public Person(bool myInitializeOK)
        {
            initializeOK = myInitializeOK;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor
        /// dataLine - line in data file from which data was read.
        /// initData - text data used to initialize Person object.
        // Person constructor with initialization data string - five comma delimited fields:
        // (1) gender ("male" or "female"), (2) start age (years), (3) haveDiarrhea (boolean 0, 1),
        // (4) haveAnemia (boolean 0, 1), (5) haveStunting (boolean 0, 1).
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(int dataLine, string initData)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Parse input record into fields by looking for comma delimiter
            string[] personFields = Regex.Split(initData, fieldDelimiter);

            // Return immediately if there are not enough fields to read in
            if (personFields.Length < numPersonFields)
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Not able to find {1} data fields.",
                    dataLine, numPersonFields);
                Console.WriteLine("Data: {0}", initData);
                Console.WriteLine("");
                return;
            }

            // (1) Try to read in gender ("female" or "male")
            if (!genderTypeList.ContainsKey(((personFields[gender_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid gender.",
                    dataLine, gender_offset + 1);
                Console.WriteLine("Data: {0}", initData);
                return;
            }
            gender = (GenderType)
                genderTypeList[((personFields[gender_offset]).Trim()).ToUpper()];

            // (2) Try to read in start age (numeric)
            if (!Double.TryParse((personFields[ageStart_offset]), out this.ageStart))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (start age) is not numeric.",
                    dataLine, ageStart_offset + 1);
                Console.WriteLine("Data: {0}", initData);
            }

            // (3) Try to read in haveDiarrhea (boolean).  Handles numeric or "true"/"false" text.
            double tempVal;
            if (Double.TryParse((personFields[haveDiarrhea_offset]), out tempVal))
            {
                this.haveDiarrheaStart = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((personFields[haveDiarrhea_offset]), out this.haveDiarrheaStart))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have diarrhea) is not boolean.",
                        dataLine, haveDiarrhea_offset + 1);
                    Console.WriteLine("Data: {0}", initData);
                }
            }

            // (4) Try to read in haveAnemia (boolean).  Handles numeric or "true"/"false" text.
            if (Double.TryParse((personFields[haveAnemia_offset]), out tempVal))
            {
                this.haveAnemiaStart = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((personFields[haveAnemia_offset]), out this.haveAnemiaStart))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have anemia) is not boolean.",
                        dataLine, haveAnemia_offset + 1);
                    Console.WriteLine("Data: {0}", initData);
                }
            }

            // (5) Try to read in haveStunting (boolean).  Handles numeric or "true"/"false" text.
            if (Double.TryParse((personFields[haveStunting_offset]), out tempVal))
            {
                this.haveStuntingStart = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((personFields[haveStunting_offset]), out this.haveStuntingStart))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have stunting) is not boolean.",
                        dataLine, haveStunting_offset + 1);
                    Console.WriteLine("Data: {0}", initData);
                }
            }

            // Initialize the "Now" fields
            ageNow = ageStart;
            haveDiarrheaNow = haveDiarrheaStart;
            haveAnemiaNow = haveAnemiaStart;
            haveStuntingNow = haveStuntingStart;
            alive = true;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - copies an existing Person.
        /// Person - Person to be copied.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(Person personToBeCopied)
        {
            initializeOK = personToBeCopied.initializeOK;
            ageStart = personToBeCopied.ageStart;
            ageNow = personToBeCopied.ageNow;
            haveDiarrheaStart = personToBeCopied.haveDiarrheaStart;
            haveDiarrheaNow = personToBeCopied.haveDiarrheaNow;
            haveAnemiaStart = personToBeCopied.haveAnemiaStart;
            haveAnemiaNow = personToBeCopied.haveAnemiaNow;
            haveStuntingStart = personToBeCopied.haveStuntingStart;
            haveStuntingNow = personToBeCopied.haveStuntingNow;
            alive = personToBeCopied.alive;
            LY = personToBeCopied.LY;
            LYDisc = personToBeCopied.LYDisc;
            QALYs = personToBeCopied.QALYs;
            costs = personToBeCopied.costs;
            gender = personToBeCopied.gender;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - Uses input from Disease RR file to create a person object
        /// that stores relative risk as a function of disease state.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(DiseaseRRPersonInitializer x)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Set disease states
            haveDiarrheaStart = x.haveDiarrhea;
            haveDiarrheaNow = x.haveDiarrhea;
            haveAnemiaStart = x.haveAnemia;
            haveAnemiaNow = x.haveAnemia;
            haveStuntingStart = x.haveStunting;
            haveStuntingNow = x.haveStunting;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - Uses input from Mortality risk file to create a person object
        /// that stores relative risk as a function of start age (which maps 1-1 to birth calendar
        /// year), current age, and gender.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(MortRiskPersonInitializer x)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Set disease states
            ageStart = x.ageStart;
            ageNow = x.ageNow;
            gender = x.gender;
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        // get functions - returns values of object members that are private and hence cannot
        // be accessed by other classes.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public bool getInitalizeOK() { return initializeOK; }
        public bool getHaveDiarrheaNow() { return haveDiarrheaNow; }
        public bool getHaveAnemiaNow() { return haveAnemiaNow; }
        public bool getHaveStuntingNow() { return haveStuntingNow; }
        public double getAgeStart() { return ageStart; }
        public double getAgeNow() { return ageNow; }
        public GenderType getGender() { return gender; }


        ///<summary> ////////////////////////////////////////////////////////////////////////////
        /// simSubject()
        /// Causes Person object to run through simulation: (1) Person object initializes 
        /// its fields, and (2) loops through lifetime at user-specified time steps.
        ///<summary> ////////////////////////////////////////////////////////////////////////////
        public void simMyLife(Policy myPolicy)
        {
            initializeMyself(); // Initialize Person object before simulating - e.g., zero out QALYs
            while (alive) ageMyself(myPolicy, SimModel.deltaTime);
            if (SimModel.debugOn)
            {
                Console.Write("Hit any key to continue");
                Console.ReadKey();
            }
        }



        ///<summary> ////////////////////////////////////////////////////////////////////////////
        /// initializeMySelf()
        /// Initializes fields that are reset at the beginning of the simulation, like life 
        /// lived.
        ///<summary> ////////////////////////////////////////////////////////////////////////////
        private void initializeMyself()
        {
            ageNow = ageStart;
            haveDiarrheaNow = haveDiarrheaStart;
            haveAnemiaNow = haveAnemiaStart;
            haveStuntingNow = haveStuntingStart;
            alive = true;
        }

        ///<summary> ////////////////////////////////////////////////////////////////////////////
        /// initializeMySelf()
        /// Initializes fields that are reset at the beginning of the simulation, like life 
        /// lived.
        ///<summary> ////////////////////////////////////////////////////////////////////////////
        private void ageMyself(Policy myPolicy, double deltaStep)
        {
            // Demonstration code - accessing values for computation of mortality risk... baseline
            // risk and relative risk.  Just need to work in Policy risk modification.  Can do that 
            // later.  FOR NOW - multiply baseline mortality and relative risk, test if person
            // dies, and move on.  

            // At each point in time: (1) compute mortality risk and check if person dies;
            // (2) compute chance of switching off disease state; (3) update accumulation
            // of life years - taking into account whether person has been born yet; (4)
            // update ageNow.  Note - I have included a field to accumulate life years.  We
            // should add another one to accumulate discounted life years.

            // Simulate until person dies... Here's how to get the numbers needed
            double policyImpact = myPolicy.policyImpact;
            Policy.PolicyType PolicyCategory = myPolicy.policyCategory;
            double myRisk = MortRisk.mortRiskTable[this];
            double diseaseRR = DiseaseRR.diseaseRRTable[this];

            Console.WriteLine("Baseline mortality: {0}", myRisk);
            Console.WriteLine("Disease RR: {0}", diseaseRR);

            Console.ReadKey();

            // TO BE DONE
            // TO BE DONE
            // TO BE DONE
            // TO BE DONE
            // TO BE DONE
            // TO BE DONE
            // TO BE DONE
            // TO BE DONE - Big task will be to use age, disease status, and policy to look up
            // probability of death.  Then call on SimRandom bernouli(p) function to determine if
            // person dies.  ALSO - have to increment life years and discounted life years (fields
            // not yet created).  Have to do discounting.  I have code for that from other simulations.
        }

        ///<summary> ////////////////////////////////////////////////////////////////////////////
        /// Keep track of outcomes that are accumulatated continuously, including QALYs and 
        /// costs incurred per unit time.
        /// deltaTime = time duration between events
        /// startTime = time of last event - i.e., time when this accumulation step started
        ///<summary> ////////////////////////////////////////////////////////////////////////////
        private void GetContinuousOutcomes(decimal myDeltaTime, decimal startTime)
        {
            double periodMidPt = (double)(startTime + myDeltaTime / 2);
            decimal discountFactor = (decimal)Math.Pow(1 + SimModel.discountRatePerYr, -periodMidPt);

            // Increment LY, LYDisc, QALYs, costs, taking into account myDeltaTime and discountFactor.
            // Check if person has been born yet (age less than zero).

            //
            //
            //

        }
    }
}


