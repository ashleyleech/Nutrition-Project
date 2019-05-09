using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Nutrition01
{
    public static class DiseasePrev
    {
        public static Dictionary<Person, double> DiseasePrevTable =
            new Dictionary<Person, double>(new PersonDISEASE_PREVComparer());

        public static Person DiseasePrevTablePerson(int lineNum, string inputData, Person.diseaseType myDisease)
        {
            DiseasePrevPersonInitializer myDiseasePrevPersonInitializer =
                new DiseasePrevPersonInitializer(lineNum, inputData, myDisease);
            if (!myDiseasePrevPersonInitializer.initializeOK) return new Person(false);
            return new Person(myDiseasePrevPersonInitializer);
        }

        public static double DiseasePrevTableVALUE(int lineNum, string inputData, Person.diseaseType myDisease)
        {
            return DiseasePrevVALUEInitializer.getPrevValue(lineNum, inputData, myDisease);

        }
    }


    public class DiseasePrevPersonInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        static int country_offset = 0;
        static int yearnow_offset = 1;
        static int agenow_offset = 2;

        public bool initializeOK;

        public Person.countrytype country;
        public int yearNow;
        public double ageNow;
        public Person.diseaseType disease;



        public DiseasePrevPersonInitializer()
        {
            initializeOK = true;
        }

        public DiseasePrevPersonInitializer(int lineNum, string inputData, Person.diseaseType myDisease)
        {
            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            initializeOK = true;

            //  (1) Read in country (string - convertable to Person countrytype).
            //  Valid values are: "ETH, IND, GHA, BGD, NPL, NGA, UGA, [or] TZA."
            if (!Person.countrytypelist.ContainsKey(((myFields[country_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid country.",
                    lineNum, country_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }
            this.country = (Person.countrytype)
                Person.countrytypelist[((myFields[country_offset]).Trim()).ToUpper()];

            // (2) Read in year (int)
            if (!int.TryParse((myFields[yearnow_offset]), out this.yearNow))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (yearnow) is not an integer.",
                    lineNum, yearnow_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }

            // (3) Read in age (double).
            if (!Double.TryParse((myFields[agenow_offset]), out this.ageNow))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (agenow) is not a double.",
                    lineNum, agenow_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
            }

            // (4) Get disease type
            disease = myDisease;

        }

    }

    public static class DiseasePrevVALUEInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        public static int PrevValue_offsetStart = 3;
        static int PrevValue_offset;
        
        public static double getPrevValue(int lineNum, string inputData, Person.diseaseType myDisease)
        {

            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            // Get offset for prevalence corresponding to myDisease
            if (Person.getPrevOffset(myDisease) == 6) Console.WriteLine();
            PrevValue_offset = Person.getPrevOffset(myDisease);
            
            double PrevValue;
            if (!Double.TryParse((myFields[PrevValue_offset]), out PrevValue))
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (prevalence) is not numeric.",
                    lineNum, PrevValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return -1;
            }

            if (PrevValue < 0 || PrevValue > 1)
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (relative risk) not from 0 to 1.",
                        lineNum, PrevValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return -1;
            }
            return PrevValue;
        }
    }


    /// <summary>
    /// Compares country, year, & age - returns true if all equal. Rounds age values 
    /// to nearest integer. 
    ///  
    /// </summary>
    public class PersonDISEASE_PREVComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            if (ReferenceEquals(null, x) && !ReferenceEquals(null, y)) return false;
            if (!ReferenceEquals(null, x) && ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(null, x) && ReferenceEquals(null, y)) return true;


            return
                (x.getCountry() == y.getCountry()) &&
                ((int)x.getYearNowForDiseaseLookup() == (int)y.getYearNowForDiseaseLookup()) &&
                ((int)x.getIntAgeNowForDiseaseLookup() == (int)y.getIntAgeNowForDiseaseLookup()) &&
                (x.getDisease() == y.getDisease());
        }

        public int GetHashCode(Person x)
        {
            // If x is null, then hash code is zero.
            if (x == null) return 0;

            int myHashValue =
                ((int)(x.getCountry())).GetHashCode() +
                x.getYearNowForDiseaseLookup().GetHashCode() +
                x.getIntAgeNowForDiseaseLookup().GetHashCode() +
                ((int)(x.getDisease())).GetHashCode();

            return myHashValue;
        }
    }
}