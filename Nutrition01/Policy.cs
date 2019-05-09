using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Nutrition01
{
    public class Policy
    {
        // Delimiter
        static String fieldDelimiter = @","; // Delimiter for elements within a list

        // Lists
        public static List<Policy> PolicyList = new List<Policy>();

        // Fields - Policy definition values
        private bool initializeOK;
        public string policyName;
        public Person.countrytype country;
        public double RR_mortality;
        public double RR_diarrhea;
        public double RR_stunting;
        public double RR_anemia;


        // Field locations in ititialization data vector
        static int policyName_offset = 0;
        static int country_offset = 1;
        public static int RRmortality_offset = 2;
        public static int RRdiarrhea_offset = 3;
        public static int RRstunting_offset = 4;
        public static int RRanemia_offset = 5;
        static int numPolicyFields = 6;


        // Constructors
        public Policy(string inputData, int LineNum)
        {
            // Set InitializeOK flag to true;
            initializeOK = true;

            // Parse input record into fields by looking for comma delimiter
            string[] policyFields = Regex.Split(inputData, fieldDelimiter);

            // Return immediately if there are not enough fields to read in
            if (policyFields.Length < numPolicyFields)
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Not able to find {1} data fields.",
                    LineNum, numPolicyFields);
                Console.WriteLine("Data: {0}", inputData);
                Console.WriteLine("");
                return;
            }


            // (1) Policy name... cannot be empty string
            if ((((policyFields[policyName_offset]).Trim()).Length) == 0)
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Policy name, Field {1}, is blank.",
                    LineNum, policyName_offset+1);
                Console.WriteLine("Data: {0}", inputData);
                Console.WriteLine("");
                return;
            }
            policyName = (policyFields[policyName_offset]).Trim();


            // (2) Read in policy COUNTRY
            if (!Person.countrytypelist.ContainsKey(((policyFields[country_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid country.",
                    LineNum, country_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }
            country = (Person.countrytype) 
                Person.countrytypelist[((policyFields[country_offset]).Trim()).ToUpper()];


            // (3) Read in RRMortality 
            if (!Double.TryParse((policyFields[RRmortality_offset]), out this.RR_mortality))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (RR_diarrhea) is not a double.",
                    LineNum, RRmortality_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }


            // (4) Read in RRdiarrhea
            if (!Double.TryParse((policyFields[RRdiarrhea_offset]), out this.RR_diarrhea))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (RR_diarrhea) is not a double.",
                    LineNum, RRdiarrhea_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }


            // (5) Read in RRstunting
            if (!Double.TryParse((policyFields[RRstunting_offset]), out this.RR_stunting))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (RR_stunting) is not a double.",
                    LineNum, RRstunting_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }


            // (6) Read in RRanemia
            if (!Double.TryParse((policyFields[RRanemia_offset]), out this.RR_anemia))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (RR_anemia) is not a double.",
                    LineNum, RRanemia_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }
            
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        // get functions - returns values of object members that are private and hence cannot
        // be accessed by other classes.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        public bool getInitalizeOK() { return initializeOK; }

    }
}
