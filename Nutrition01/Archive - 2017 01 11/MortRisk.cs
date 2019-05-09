using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Nutrition01
{
    public static class MortRisk
    {
        public static Dictionary<Person, double> mortRiskTable =
            new Dictionary<Person, double>(new PersonMortRiskComparer());

        public static Person MortRiskTablePerson(int lineNum, string inputData)
        {
            MortRiskPersonInitializer myMortRiskPersonInitializer =
                new MortRiskPersonInitializer(inputData, lineNum);
            if (!myMortRiskPersonInitializer.initializeOK) return new Person(false);
            return new Person(myMortRiskPersonInitializer);
        }

        public static double MortRiskTableRRValue(int lineNum, string inputData)
        {
            return MortRiskValueInitializer.getRRValue(inputData, lineNum);
        }
    }

    public class MortRiskPersonInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        static int ageStart_offset = 2;
        static int ageNow_offset = 3;
        static int gender_offset = 4;

        public double ageStart;
        public double ageNow;
        public Person.GenderType gender;
        public bool initializeOK;

        public MortRiskPersonInitializer()
        {
            initializeOK = true;
        }

        public MortRiskPersonInitializer(string inputData, int dataLine)
        {
            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            initializeOK = true;

            // (1) Try to read in ageStart (double).
            if (!Double.TryParse((myFields[ageStart_offset]), out this.ageStart))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (age at start) is not a double.",
                    dataLine, ageStart_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }

            // (2) Try to read in ageNow (double).
            if (!Double.TryParse((myFields[ageNow_offset]), out this.ageNow))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (age now) is not a double.",
                    dataLine, ageNow_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }

            // (3) Try to read in gender (string - convertable to Person genderType).
            // Valid values are "male" or "female" - not case sensitive.
            if (!Person.genderTypeList.ContainsKey(((myFields[gender_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid gender.",
                    dataLine, gender_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }
            gender = (Person.GenderType)
                Person.genderTypeList[((myFields[gender_offset]).Trim()).ToUpper()];
        }
    }

    public static class MortRiskValueInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        static int mortRiskValue_offset = 5;

        public static double getRRValue(string inputData, int dataLine)
        {
            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            double mortRiskValue;
            if (!Double.TryParse((myFields[mortRiskValue_offset]), out mortRiskValue))
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (relative risk) is not numeric.",
                    dataLine, mortRiskValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return -1;
            }
            if (mortRiskValue < 0 || mortRiskValue > 1)
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (relative risk) not from 0 to 1.",
                    dataLine, mortRiskValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return -1;
            }
            return mortRiskValue;
        }
    }

    public class PersonMortRiskComparer : IEqualityComparer<Person>
    {
        /// <summary>
        /// Compares ageStart, ageNow, gender - returns true if all equal.  Rounds age values to
        /// nearest integer.  
        /// </summary>
        public bool Equals(Person x, Person y)
        {
            if (ReferenceEquals(null, x) && !ReferenceEquals(null, y)) return false;
            if (!ReferenceEquals(null, x) && ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(null, x) && ReferenceEquals(null, y)) return true;
            return
                ((int)x.getAgeStart() == (int)y.getAgeStart()) &&
                ((int)x.getAgeNow() == (int)y.getAgeNow()) &&
                (x.getGender() == y.getGender());
        }

        public int GetHashCode(Person x)
        {
            if (x == null) return 0;
            int hash =
                x.getAgeStart().GetHashCode() +
                x.getAgeNow().GetHashCode() +
                (Convert.ToDouble(x.getGender())).GetHashCode();
            return hash;
        }
    }

}



