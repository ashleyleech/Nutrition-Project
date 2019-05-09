using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Nutrition01
{
    public static class Parms
    {
        static String parmNameDelimiter = @"/"; // Delimiter separating parameter name and value
        static String parmListDelimiter = @"\|"; // Delimiter separating parameters following "/"
        static String parmWithinListDelimiter = @","; // Delimiter for elements within a list

        // Sorted dictionary containing the defaults for each parameter
        static SortedDictionary<string, List<List<double>>> parmsList =
            new SortedDictionary<string, List<List<double>>>();

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// addNewParm
        /// Takes a line of text and the file line number.  Attempts to parse the input text line
        /// as parmName/parameter list.  Returns false if it fails.
        /// </summary> ////////////////////////////////////////////////////////////////////////
        public static bool addNewParm(string inputRecord, int lineNumber)
        {
            string parmName;
            string parmString;
            List<List<double>> parmList;

            // If string is empty, return without doing anything
            if (inputRecord.Length == 0) return true;

            // Split the input into two strings (delimited by "/").  The first element is the parameter
            // name and the second is the list of parameters
            string[] parmsUncoverted = Regex.Split(inputRecord, parmNameDelimiter);

            // Error - cannot find parameter specification following parameter delimiter
            if ((parmsUncoverted.Length == 1))
            {
                Console.WriteLine("Problem with File 04 (parameter file) at line \"{0}\".", lineNumber);
                Console.WriteLine("Input does not contain \"{0}\" followed by parameter specification.",
                    parmNameDelimiter);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            // Error - too many fields specified - should be only two - a parameter name and, following
            // the parameter delimiter, a parameter specification.
            if ((parmsUncoverted.Length == 1))
            {
                Console.WriteLine("Problem with File 04 (parameter file) at line \"{0}\".", lineNumber);
                Console.WriteLine("Input contains more than a single \"{0}\" character.", parmNameDelimiter);
                Console.WriteLine("Line should contain parameter name, delimiter, and parameter specification.");
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            // Exactly two parameters - read them in.
            parmName = parmsUncoverted[0];
            parmString = parmsUncoverted[1];

            // Check if parameter name is non-blank.
            if (parmName == "")
            {
                Console.WriteLine("Problem with File 04 (parameter file) at line \"{0}\".", lineNumber);
                Console.WriteLine("Parameter name missing.", parmNameDelimiter);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            // Check if parameter specification is non-blank.
            if (parmString == "")
            {
                Console.WriteLine("Problem with File 04 (parameter file) at line \"{0}\".", lineNumber);
                Console.WriteLine("Parameter specification missing.", parmNameDelimiter);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            // Split up the list of parameters into elements, each of which is an entry in
            // an array list.
            parmList = new List<List<double>>();
            parmsUncoverted = Regex.Split(parmString, parmListDelimiter);
            if (parmsList.ContainsKey(parmName.ToLower()))
            {
                Console.WriteLine("Default file contains more than one entry for parameter \"{0}\".", parmName);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }
            parseParms(parmString, parmList);
            parmsList.Add(parmName.ToLower(), parmList);
            return true;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// parseParms
        /// Takes a line of text specifying a parameter list.  Parameters are delmited by "|".
        /// Attempts to parse the input text line
        /// If it succeeds, it returns a list of parameters.
        /// </summary> ////////////////////////////////////////////////////////////////////////
        public static void parseParms(string parmString, List<List<double>> parmList)
        {
            string[] parmsUncoverted = Regex.Split(parmString, parmListDelimiter);
            foreach (var ntuppleString in parmsUncoverted)
            {
                if (ntuppleString != "")
                {
                    List<double> ntupple = new List<double>();
                    string[] ntuppleElements = Regex.Split(ntuppleString, parmWithinListDelimiter);
                    foreach (var element in ntuppleElements)
                    {
                        double elementValue = (double)Convert.ToDouble(element);
                        ntupple.Add(elementValue);
                    }
                    parmList.Add(ntupple);
                }
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// Checks if parameter name is in the Parmlist and has a non-null value.  Exits with error 
        /// message if necessary.
        /// </summary> //////////////////////////////////////////////////////////////////////////////
        static private bool findParm(string parmName)
        {
            if (!parmsList.ContainsKey(parmName.ToLower()))
            {
                Console.WriteLine("Default file does not contain an entry for parameter \"{0}\".", parmName);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            if (parmsList[parmName.ToLower()] == null)
            {
                Console.WriteLine("Default file contains a null entry for parameter \"{0}\".", parmName);
                Console.WriteLine("Hit any key to continue.");
                Console.ReadKey();
                return false;
            }

            return true;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// getParmRaw(parmName)
        /// Takes a string as an argument.  If the parameter can be found, it returns a list of 
        /// nested lists of doubles.  Otherwise, it exits with an error message.
        /// </summary> ////////////////////////////////////////////////////////////////////////
        public static List<List<double>> getParmRaw(string parmName)
        {
            if (!findParm(parmName)) Environment.Exit(0);
            return parmsList[parmName.ToLower()];
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// getParmRaw(parmName)
        /// Takes a string as an argument.  If the parameter can be found, it returns a list of 
        /// nested lists of doubles.  Otherwise, it exits with an error message.
        /// </summary> ////////////////////////////////////////////////////////////////////////
        public static double getParmDouble(string parmName)
        {
            return (getParmRaw(parmName.ToLower()))[0][0];
        }


    }
}

