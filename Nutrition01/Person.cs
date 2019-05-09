using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRandom;
using System.Text.RegularExpressions;
using System.Collections;
using Nutrition01;

namespace Nutrition01
{
    public class Person
    {

        // Delimiter
        static String fieldDelimiter = @","; // Delimiter for elements within a list

        // Lists
        public static List<Person> Population = new List<Person>();


        // Fields - Initialization values
        private bool initializeOK;
        public double birthYear;
        public double yearNow;
        public double startAge;
        public double ageNow;
        public bool haveDiarrhea;
        public bool haveAnemia;
        public bool haveStunting;


        // Status fields
        public double yearsSinceStart;
        public bool alive;
        public static Boolean firstSimCycle;


        // Outcome fields
        public double LY;
        public double LYDisc;
        public double QALYs;
        public double ageAtDeath;
        public double costs;

        public double everStunting;
        public double everAnemia;
        public double everDiarrhea;

        public double Stunting_0;
        public double Anemia_0;
        public double Diarrhea_0;
        public double Stunting_5;
        public double Anemia_5;
        public double Diarrhea_5;

        public double isAlive_0;
        public double isAlive_5;

        public double intAgeNow;


        // Lookup key fields
        public enum diseaseType { STUNTING, DIARRHEA, ANEMIA }
        public diseaseType disease;


        // Valid field values
        public enum countrytype { ETH, IND, NGA }
        public countrytype country;
        public static Dictionary<string, int> countrytypelist = new Dictionary<string, int>()
        {
            {"ETH", 0},
            {"IND", 1},
            {"NGA", 2},  
        };

        public enum GenderType { FEMALE, MALE }
        public GenderType gender;
        public static Dictionary<string, int> genderTypeList = new Dictionary<string, int>()
        {
            {"FEMALE",0 },
            {"MALE",1 },
        };

        public static Dictionary<string, int> DiseasePrevTable = new Dictionary<string, int>()
        {

            {"STUNTING", 0},
            {"DIARRHEA", 1},
            {"ANEMIA", 2},

        };


        // PERSON field locations in ititialization data vector
        public static int gender_offset = 0;
        public static int startage_offset = 1;
        public static int country_offset = 2;
        public static int birthyear_offset = 3;
        static int numPersonFields = 4;




        // Call test function to look up prevalence
        //better to put in myperson since age is private variable, etc.


        public void myTest()
        {

            Person myPerson = new Person();
            myPerson.country = Person.countrytype.NGA;
            myPerson.yearNow = 2015;
            myPerson.ageNow = 2;
            myPerson.gender = Person.GenderType.MALE;

            myPerson.disease = Person.diseaseType.DIARRHEA;
            double myPrev = DiseasePrev.DiseasePrevTable[myPerson];

            myPerson.disease = Person.diseaseType.ANEMIA;
            myPrev = DiseasePrev.DiseasePrevTable[myPerson];

            myPerson.disease = Person.diseaseType.STUNTING;
            myPrev = DiseasePrev.DiseasePrevTable[myPerson];


        }



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
        // Person constructor with initialization data string - four comma delimited fields:
        // (1) gender, (2) startage, (3) country, (4) birthyear 
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


            // (2) Read in startage (numeric) and set ageNow to startAge
            if (!Double.TryParse((personFields[startage_offset]), out this.startAge))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (start age) is not numeric.",
                    dataLine, startage_offset + 1);
                Console.WriteLine("Data: {0}", initData);
            }
            ageNow = startAge;

            // (3) Read in country ("ETH" to "TZA")
            if (!countrytypelist.ContainsKey(((personFields[country_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid country",
                    dataLine, country_offset + 1);
                Console.WriteLine("Data: {0}", initData);
                return;
            }
            country = (countrytype)
                countrytypelist[((personFields[country_offset]).Trim()).ToUpper()];


            // (4) Read in birthyear (numeric)
            if (!Double.TryParse((personFields[birthyear_offset]), out this.birthYear))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (prevalence age) is not numeric.",
                    dataLine, birthyear_offset + 1);
                Console.WriteLine("Data: {0}", initData);
            }

        }



        /// <summary> ///////////////////////////////////////////////////////////////////////////////////
        // get functions - returns values of object members that are private and hence cannot
        // be accessed by other classes.
        /// </summary> //////////////////////////////////////////////////////////////////////

        public bool getInitalizeOK() { return initializeOK; }
        public double getyearsSinceStart() { return yearsSinceStart; }
        public Boolean isAlive() { return alive; }
        public double getAgeAtDeath() { return ageAtDeath; }
        public Person getPersonCopy() { Person p = new Person(); return p; }
        public countrytype getCountry() { return country; }

        public double getyearNow() { return yearNow; }
        public double getyearNowDisease() { return yearNow; }
        public double getYearNowForDiseaseLookup() { return Math.Min(2024, getyearNowDisease()); }

        public double getageNow() { return ageNow; }
        public double getIntAgeNow() { return ageNow + 0.5; }
        public double getIntAgeNowForDiseaseLookup() { return Math.Min(4, getIntAgeNow()); }
        public double getstartAge() { return startAge; }
        public diseaseType getDisease() { return disease; }
        public GenderType getGender() { return gender; }
        public bool gethaveDiarrhea() { return haveDiarrhea; }
        public bool gethaveAnemia() { return haveAnemia; }
        public bool gethaveStunting() { return haveStunting; }


        public static int getPrevOffset(diseaseType myDisease)
        {
            int returnVal;
            switch (myDisease)
            {
                case diseaseType.STUNTING:
                    returnVal = DiseasePrevVALUEInitializer.PrevValue_offsetStart + (int)diseaseType.STUNTING; break;
                case diseaseType.DIARRHEA:
                    returnVal = DiseasePrevVALUEInitializer.PrevValue_offsetStart + (int)diseaseType.DIARRHEA; break;
                case diseaseType.ANEMIA:
                    returnVal = DiseasePrevVALUEInitializer.PrevValue_offsetStart + (int)diseaseType.ANEMIA; break;
                default: returnVal = -1; break;
            }
            return returnVal;
        }


        public static int getgenderOffset(GenderType myGender)
        {
            int returnVal;
            switch (myGender)
            {

                case GenderType.FEMALE: returnVal = gender_offset; break;
                case GenderType.MALE: returnVal = gender_offset; break;
                default: returnVal = -1; break;

            }
            return returnVal;


        }




        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - Uses input from DISEASE PREVALENCE file to create a person object
        /// that stores Disease prevalence as a function of startage, year & country.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(DiseasePrevPersonInitializer x)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Set disease prevalence lookup field values;
            ageNow = x.ageNow;
            yearNow = x.yearNow;
            country = x.country;
            disease = x.disease;

        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - Uses input from DISEASE RR file to create a person object
        /// that stores relative risk as a function of disease state.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(DiseaseRRPersonInitializer x)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Set disease states
            haveDiarrhea = x.haveDiarrhea;
            haveAnemia = x.haveAnemia;
            haveStunting = x.haveStunting;
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// Person constructor - Uses input from MORTALITY RISK file to create a person object
        /// that stores relative risk as a function of start age (which maps 1-1 to birth calendar
        /// year), current age, and gender.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public Person(MortRiskPersonInitializer x)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Set disease states
            startAge = x.startAge;
            ageNow = x.ageNow;
            gender = x.gender;
            country = x.country;

        }


        //************************************************************************************************
        // Simulation functions
        //************************************************************************************************


        /////////////////////////////////////////////////////////////////////////
        // initializePerson - Sets 
        /////////////////////////////////////////////////////////////////////////

        public void ageMyself(Policy myPolicy)
        {
            double disprev = DiseasePrev.DiseasePrevTable[this];
            double myRisk = MortRisk.mortRiskTable[this];
            double diseaseRR = DiseaseRR.diseaseRRTable[this];


            Console.WriteLine("Baseline disease prevalence: {0}", disprev);
            Console.WriteLine("Baseline mortality: {0}", myRisk);
            Console.WriteLine("Disease RR: {0}", diseaseRR);


            Console.ReadKey();

        }


        /////////////////////////////////////////////////////////////////////////
        //Ages person by duration in the current stage
        //Steps through lifetime of person
        //If alive, probabilitly of disease & increase age
        /////////////////////////////////////////////////////////////////////////


        public void simMyLife(Policy myPolicy)
        {

            // Transition probabilities
            double probStunting;
            double probAnemia;
            double probDiarrhea;
            double mortConversionFactor;
            double baselineMortProb;


            // Outcome/Acumulator values
            LY = 0;
            LYDisc = 0;
            QALYs = 0;
            ageAtDeath = 0;

            everDiarrhea = 0;
            everAnemia = 0;
            everStunting = 0;

            Stunting_0 = 0;
            Anemia_0 = 0;
            Diarrhea_0 = 0;
            Stunting_5 = 0;
            Anemia_5 = 0;
            Diarrhea_5 = 0;

            isAlive_0 = 0;
            isAlive_5 = 0;


        // Initialize variables that have to be initialized at the beginning of life
            yearsSinceStart = (int)0;
            yearNow = SimModel.StartYear;
            firstSimCycle = true;
            alive = true;
            intAgeNow = ageNow + 0.5;


        // Check if person has been born yet (age less than zero).

        // How to get at simulation variables
            double discountRate = SimModel.discountRatePerYr;
            double deltaTime = SimModel.deltaTime;
            double NumSubjects = SimModel.Sim_NumSubjects;
            double StartYear = SimModel.StartYear;
            double maxAge = SimModel.maxAge;

            // How to get at policy variables
            double RR_mortality = myPolicy.RR_mortality;
            double RR_diarrhea = myPolicy.RR_diarrhea;
            double RR_stunting = myPolicy.RR_stunting;
            double RR_anemia = myPolicy.RR_anemia;
            this.country = myPolicy.country;

            while (this.alive)
            {
                alive = (ageNow < 100);

                // Stuff to do only after person is born...
                if (ageNow >= 0)
                {
                    // Determine if person has STUNTING
                    this.disease = diseaseType.STUNTING;
                    probStunting = DiseasePrev.DiseasePrevTable[this];
                    this.haveStunting = MyRandom.RandomProvider.NextBernoulli(Math.Min(probStunting * myPolicy.RR_stunting, 1.0));

                    // Determine if person has ANEMIA
                    this.disease = diseaseType.ANEMIA;
                    probAnemia = DiseasePrev.DiseasePrevTable[this];
                    this.haveAnemia = MyRandom.RandomProvider.NextBernoulli(Math.Min(probAnemia * myPolicy.RR_anemia, 1.0));


                    // Determine if person has DIARRHEA
                    this.disease = diseaseType.DIARRHEA;
                    probDiarrhea = DiseasePrev.DiseasePrevTable[this];
                    this.haveDiarrhea = MyRandom.RandomProvider.NextBernoulli(Math.Min(probDiarrhea * myPolicy.RR_diarrhea, 1.0));

                    // Determine if person dies now according to disease probabilities + policy
                    if (yearsSinceStart >= SimModel.maxSimDuration)
                    {
                        this.alive = false;
                    }
                    else
                    {
                        mortConversionFactor = DiseaseRR.diseaseRRTable[this];
                        baselineMortProb = MortRisk.mortRiskTable[this];
                        this.alive = !MyRandom.RandomProvider.NextBernoulli(baselineMortProb * mortConversionFactor * RR_mortality);
                    }

                    // DEBUG XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                 /*   if (ageNow == 0)
                    {
                        int have_diarrhea_int = 0;
                        int have_anemia_int = 0;
                        int have_stunting_int = 0;
                        if (this.haveDiarrhea) have_diarrhea_int = 1;
                        if (this.haveAnemia) have_anemia_int = 2;
                        if (this.haveStunting) have_stunting_int = 4;

                        int disComboNum = have_stunting_int + have_anemia_int + have_diarrhea_int;
                        SimModel.diseaseArray[disComboNum]++;
                    }*/

                    LY += SimModel.deltaTime;
                    LYDisc += SimModel.deltaTime * (1 / Math.Pow(1 + SimModel.discountRatePerYr, yearsSinceStart));

                }


                // If person ever acquires disease then set to 1
                //To determine prev of disease at age 0 and age 5

                if (haveStunting)
                {
                    everStunting = 1;
                    if (intAgeNow == 0) Stunting_0 = 1;
                    if (intAgeNow == 5) Stunting_5 = 1;
                }

                if (haveAnemia)
                {
                    everAnemia = 1;
                    if (intAgeNow == 0) Anemia_0 = 1;
                    if (intAgeNow == 5) Anemia_5 = 1;
                }


                if (haveDiarrhea)
                {
                    everDiarrhea = 1;
                    if (intAgeNow == 0) Diarrhea_0 = 1;
                    if (intAgeNow == 5) Diarrhea_5 = 1;
                }



                if ((alive) && (intAgeNow >= 0 || intAgeNow <1))
                    isAlive_0 = 1;

                if ((alive) && (intAgeNow >= 5 || intAgeNow <6))
                    isAlive_5 = 1;

                

                // Increment years taking into account myDeltaTime and discountFactor
                // deltaTime = time duration between events
                // Don't have QALYs yet 
                yearsSinceStart += SimModel.deltaTime;
                yearNow = SimModel.StartYear + yearsSinceStart;
                ageNow += SimModel.deltaTime;


                // Flag that the subject is on the first cycle through the model
                firstSimCycle = false;

            }

        }

    /// <summary> //////////////////////////////////////////////////////////////////////////
    /// Person constructor - copies an existing Person.
    /// Person - Person to be copied.
    /// </summary> /////////////////////////////////////////////////////////////////////////

    public Person(Person personToBeCopied)
    {
        initializeOK = personToBeCopied.initializeOK;
        birthYear = personToBeCopied.birthYear;
        yearNow = personToBeCopied.yearNow;
        startAge = personToBeCopied.startAge;
        ageNow = personToBeCopied.ageNow;
        haveDiarrhea = personToBeCopied.haveDiarrhea;
        haveAnemia = personToBeCopied.haveAnemia;
        haveStunting = personToBeCopied.haveStunting;
        gender = personToBeCopied.gender;
        country = personToBeCopied.country;
        disease = personToBeCopied.disease;
        alive = personToBeCopied.alive;
        LY = personToBeCopied.LY;
        LYDisc = personToBeCopied.LYDisc;
        QALYs = personToBeCopied.QALYs;
        everStunting = personToBeCopied.everStunting;
        everAnemia = personToBeCopied.everAnemia;
        everDiarrhea = personToBeCopied.everDiarrhea;
        Stunting_0 = personToBeCopied.Stunting_0;
        Anemia_0 = personToBeCopied.Anemia_0;
        Diarrhea_0 = personToBeCopied.Diarrhea_0;
        Stunting_5 = personToBeCopied.Stunting_5;
        Anemia_5 = personToBeCopied.Anemia_5;
        Diarrhea_5 = personToBeCopied.Diarrhea_5;
        isAlive_0 = personToBeCopied.isAlive_0;
        isAlive_5 = personToBeCopied.isAlive_5;


        //costs = personToBeCopied.costs;

        }

    }

}



   
    

    


