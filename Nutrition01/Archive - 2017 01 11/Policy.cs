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
        public double policyImpact;

        public enum PolicyType { MORTALITY, DIARRHEA, ANEMIA, STUNTING }
        public PolicyType policyCategory;
        static Dictionary<string, int> policyTypeList = new Dictionary<string, int>()
        {
            {"MORTALITY",0 },
            {"DIARRHEA",1 },
            {"ANEMIA",2 },
            {"STUNTING",3 }
        };

        // Field locations in ititialization data vector
        static int policyName_offset = 0;
        static int policyCategory_offset = 1;
        public static int lowestDoseForEffect_offset = 2;
        public static int policyImpact_offset = 3;
        static int numPolicyFields = 4;

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

            // (1) Policy name... cannot be empty string.
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

            // (2) Check if policy category field is in the list of valid categories
            if (!policyTypeList.ContainsKey(((policyFields[policyCategory_offset]).Trim()).ToUpper()))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} is not a valid policy type.",
                    LineNum, policyCategory_offset + 1);
                Console.WriteLine("Data: {0}", inputData);
                return;
            }
            policyCategory = (PolicyType) 
                policyTypeList[((policyFields[policyCategory_offset]).Trim()).ToUpper()];

            // (3) Check if the data input contains a disease relative risk value
            if (!Double.TryParse((policyFields[policyImpact_offset]), out policyImpact))
            {
                initializeOK = false;
                Console.WriteLine("Error - Data line: {0} - Field {1} (policy impact) is not numeric.",
                    LineNum, policyImpact_offset + 1);
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
