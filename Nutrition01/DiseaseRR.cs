using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Nutrition01
{
    public static class DiseaseRR
    {
        public static Dictionary<Person, double> diseaseRRTable =
            new Dictionary<Person, double>(new PersonDiseaseRRComparer());

        public static Person DiseaseRRTablePerson(int lineNum, string inputData)
        {
            DiseaseRRPersonInitializer myDiseaseRRPersonInitializer =
                new DiseaseRRPersonInitializer(inputData, lineNum);
            if (!myDiseaseRRPersonInitializer.initializeOK) return new Person(false);
            return new Person(myDiseaseRRPersonInitializer);
        }

        public static double DiseaseRRTableRRValue(int lineNum, string inputData)
        {
            return DiseaseRRValueInitializer.getRRValue(inputData, lineNum);
        }
    }

    public class DiseaseRRPersonInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        static int haveStunting_offset = 0;
        static int haveDiarrhea_offset= 2;
        static int haveAnemia_offset = 1;

        public bool haveDiarrhea;
        public bool haveAnemia;
        public bool haveStunting;
        public bool initializeOK;

        public DiseaseRRPersonInitializer()
        {
            initializeOK = true;
        }

        public DiseaseRRPersonInitializer(string inputData, int dataLine)
        {
            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            initializeOK = true;

            // (1) Try to read in haveDiarrhea (boolean).  Handles numeric or "true"/"false" text.
            double tempVal;
            if (Double.TryParse((myFields[haveDiarrhea_offset]), out tempVal))
            {
                this.haveDiarrhea = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((myFields[haveDiarrhea_offset]), out this.haveDiarrhea))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have diarrhea) is not boolean.",
                        dataLine, haveDiarrhea_offset + 1);
                    Console.WriteLine("Data: {0}", inputData);
                }
            }

            // (2) Try to read in haveAnemia (boolean).  Handles numeric or "true"/"false" text.
            if (Double.TryParse((myFields[haveAnemia_offset]), out tempVal))
            {
                this.haveAnemia = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((myFields[haveAnemia_offset]), out this.haveAnemia))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have anemia) is not boolean.",
                        dataLine, haveAnemia_offset + 1);
                    Console.WriteLine("Data: {0}", inputData);
                }
            }

            // (3) Try to read in haveStunting (boolean).  Handles numeric or "true"/"false" text.
            if (Double.TryParse((myFields[haveStunting_offset]), out tempVal))
            {
                this.haveStunting = Convert.ToBoolean(tempVal);
            }
            else
            {
                if (!Boolean.TryParse((myFields[haveStunting_offset]), out this.haveStunting))
                {
                    initializeOK = false;
                    Console.WriteLine("Error - Data line: {0} - Field {1} (have stunting) is not boolean.",
                        dataLine, haveStunting_offset + 1);
                    Console.WriteLine("Data: {0}", inputData);
                }
            }
        }
    }

    public static class DiseaseRRValueInitializer
    {
        static String fieldDelimiter = @","; // Standard field delimiter for CSV files
        static int rrValue_offset = 5;

        public static double getRRValue(string inputData, int dataLine)
        {
            // Parse input record into fields by looking for comma delimiter
            string[] myFields = Regex.Split(inputData, fieldDelimiter);

            double rrValue;
            if (!Double.TryParse((myFields[rrValue_offset]), out rrValue))
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (relative risk) is not numeric.",
                    dataLine, rrValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return -1;
            }
            if (rrValue < 0)
            {
                Console.WriteLine("Error - Data line: {0} - Field {1} (relative risk) < 0.",
                    dataLine, rrValue_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return rrValue;
            }
            return rrValue;
        }
    }

    public class PersonDiseaseRRComparer : IEqualityComparer<Person>
    {
        /// <summary>
        /// Compares haveDiarrheaNow, haveAnemiaNow, haveStuntingNow - returns true if all equal
        /// </summary>
        public bool Equals(Person x, Person y)
        {
            if (ReferenceEquals(null, x) && !ReferenceEquals(null, y)) return false;
            if (!ReferenceEquals(null, x) && ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(null, x) && ReferenceEquals(null, y)) return true;
            return 
                x.gethaveDiarrhea() == y.gethaveDiarrhea() &&
                x.gethaveAnemia() == y.gethaveAnemia() &&
                x.gethaveStunting() == y.gethaveStunting();
        }

        public int GetHashCode(Person x)
        {
            if (x == null) return 0;
            double hashInput =
                1 * Convert.ToDouble(x.gethaveDiarrhea()) +
                2 * Convert.ToDouble(x.gethaveAnemia()) +
                4 * Convert.ToDouble(x.gethaveStunting());
            return hashInput.GetHashCode();
        }
    }
}
