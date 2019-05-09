using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Nutrition01
{
    static class SimIO
    {
        static string file01_Population_name;
        static string file02_MortRisk_name;
        static string file03_DiseaseRR_name;
        static string file04_Parameters_name;
        static string file05_Policies_name;

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// InitalizeIO
        /// Tries to open the file specified on the command line.  If it succeeds, it reads 
        /// three lines and inserts the name strings for the three input files for the simulation.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static public bool InitializeIO(string[] CommandLineArgs)
        {
            if (!readCommandLineFile(CommandLineArgs)) return false;
            if (!otherFilesExist()) return false;
            if (!readOtherFiles()) return false;

            return true;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readCommandLineFile
        /// Tries to open the command line file.  Returns false if it cannot open the file 
        /// specified on the command line.  If it can open that file, it reads in the first four
        /// lines of data and stores the file names in class static variables.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static public bool readCommandLineFile(string[] CommandLineArgs)
        {
            StreamReader fileReader;
            FileStream myFile;

            // Check if program call had one command line argument
            if (CommandLineArgs.Length != 1)
            {
                Console.WriteLine("Error - Command line should have 1 argument.");
                Console.WriteLine("Actual number of arguments was: {0}", CommandLineArgs.Length);
                Console.WriteLine("");
                Console.WriteLine("Command line should specify a file containing file path and name for each of the following:");
                Console.WriteLine("1. Population file");
                Console.WriteLine("2. Baseline mortality risk file");
                Console.WriteLine("3. Disease relative risk file");
                Console.WriteLine("4. Parameter file");
                Console.WriteLine("5. Policy specification file");
                Console.WriteLine("");
                Console.Write("Hit any key to continue");
                Console.ReadKey();
                return false;
            }

            // Try to open the file specified on the command line
            if (!File.Exists(CommandLineArgs[0]))
            {
                Console.WriteLine("Error - Could not open file specified on command line.");
                Console.WriteLine("File name was: {0}", CommandLineArgs[0]);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // Opening the command line argument file worked... start reading it
            myFile = new FileStream(CommandLineArgs[0], FileMode.Open, FileAccess.Read);
            fileReader = new StreamReader(myFile);

            // Read each line and pass it to the function that opens the file it is supposed
            // to refer to.
            file01_Population_name = fileReader.ReadLine();
            file02_MortRisk_name = fileReader.ReadLine();
            file03_DiseaseRR_name = fileReader.ReadLine();
            file04_Parameters_name = fileReader.ReadLine();
            file05_Policies_name = fileReader.ReadLine();

            return true;
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// otherFilesExist
        /// Checks if files specified in command line file exist.  Returns false if any does not.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool otherFilesExist()
        {
            bool returnVal = true;

            // See if files specified in command line file exist... report error message and 
            // set returnVal to false if something is not found.
            if (!File.Exists(file01_Population_name))
            {
                otherFilesExist_errorMsg("Population file", file01_Population_name, 1);
                returnVal = false;
            }

            if (!File.Exists(file02_MortRisk_name))
            {
                otherFilesExist_errorMsg("Mortality risk file", file02_MortRisk_name, 2);
                returnVal = false;
            }

            if (!File.Exists(file03_DiseaseRR_name))
            {
                otherFilesExist_errorMsg("Disease relative risk", file03_DiseaseRR_name, 3);
                returnVal = false;
            }

            if (!File.Exists(file04_Parameters_name))
            {
                otherFilesExist_errorMsg("Parameter file", file04_Parameters_name, 4);
                returnVal = false;
            }

            if (!File.Exists(file05_Policies_name))
            {
                otherFilesExist_errorMsg("Policy specification file", file05_Policies_name, 5);
                returnVal = false;
            }

            if (!returnVal) Console.ReadKey();
            return returnVal;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// otherFilesExist_errorMsg
        /// Displays error message for file not found - including name of file and command line
        /// file line number that is supposed to have the name of the missing file.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private void otherFilesExist_errorMsg(string fileWanted, string fileName, int lineNumber)
        {
            if (fileName == null) fileName = "left blank";
            Console.WriteLine("Error - Looking for {0}.", fileWanted);
            Console.WriteLine("Line {0} in command line file specifies file: {1}.", lineNumber, fileName);
            Console.WriteLine("");
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readOtherFile
        /// Tries to read the files specified in the command line file.  If it runs into a problem 
        /// reading a file, it reports the error and returns false.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool readOtherFiles()
        {
            if (!readFile01_population(file01_Population_name)) return false;
            if (!readFile02_mortRisk(file02_MortRisk_name)) return false;
            if (!readFile03_diseaseRR(file03_DiseaseRR_name)) return false;
            if (!readFile04_Parameters(file04_Parameters_name)) return false;
            if (!readFile05_policies(file05_Policies_name)) return false;
            return true;
        }



        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readFile01_population
        /// Open and read in File01 - Population file.  Create Population list - one Person object
        /// per line of data read in.  Report out to console any problems.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool readFile01_population(string fileName)
        {
            FileStream myFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(myFile);
            int lineNum = 2; // Keep track of where we are in the file in case there is a problem

            // Read in first two lines of file (because first line is column headings to be ignored).
            string inputRecord = fileReader.ReadLine(); inputRecord = fileReader.ReadLine();

            // Read until reaching the end of file.  Report errors and exit if any line has bad data
            while (inputRecord != null)
            {
                if (inputRecord != "")
                {
                    Person myPerson = new Person(lineNum, inputRecord);
                    if (!myPerson.getInitalizeOK())
                    {
                        Console.WriteLine("Hit any key");
                        Console.ReadKey();
                        return false;
                    }
                    Person.Population.Add(myPerson);
                }
                inputRecord = fileReader.ReadLine();
                lineNum++;
            }

            // If file is empty, return with error
            if (Person.Population.Count() == 0)
            {
                Console.WriteLine("Error - File contains no data.");
                Console.WriteLine("File name was: {0}.", fileName);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // No errors encountered - so return true
            return true;
        }

        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readFile02_mortRisk
        /// Open and read in File02 - Mortality file.  Create lookup table of mortality risks as
        /// a function of gender, age, and calendar year born.  Report out to console any problems.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool readFile02_mortRisk(string fileName)
        {
            FileStream myFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(myFile);
            int lineNum = 2; // Keep track of where we are in the file in case there is a problem

            // Read in first two lines of file (because first line is column headings to be ignored).
            string inputRecord = fileReader.ReadLine(); inputRecord = fileReader.ReadLine();

            // Read until reaching the end of file.  Report errors and exit if any line has bad data
            while (inputRecord != null)
            {
                if (inputRecord != "")
                {
                    // Get disease state fields, create a Person object initialized with this
                    // set of diseases.
                    Person myPerson = MortRisk.MortRiskTablePerson(lineNum, inputRecord);
                    if (!myPerson.getInitalizeOK())
                    {
                        Console.WriteLine("File name was: {0}.", fileName);
                        Console.WriteLine("Hit any key"); Console.ReadKey(); return false;
                    }

                    // Get relative risk, create a double initialized with this value.
                    double mortRisk = MortRisk.MortRiskTableRRValue(lineNum, inputRecord);
                    if (mortRisk < 0)
                    {
                        Console.WriteLine("Hit any key"); Console.ReadKey(); return false;
                    }

                    if (MortRisk.mortRiskTable.ContainsKey(myPerson))
                    {
                        Console.WriteLine("Error - Mortality risk for start age, current age, and gender entered twice.");
                        Console.WriteLine("File name was: {0}.", fileName);
                        Console.WriteLine("Entry: {0}", inputRecord);
                        Console.WriteLine("");
                        Console.ReadKey();
                        return false;
                    }

                    // Add the Person object (key) and relative risk (lookup value) to the dictionary.
                    MortRisk.mortRiskTable.Add(myPerson, mortRisk);
                }
                inputRecord = fileReader.ReadLine();
                lineNum++;
            }

            // If diseaseRRTable is empty, return with error
            if (MortRisk.mortRiskTable.Count() == 0)
            {
                Console.WriteLine("Error - File contains no data.");
                Console.WriteLine("File name was: {0}.", fileName);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // No errors encountered - so return true
            return true;
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readFile03_diseaseRR
        /// Open and read in File03 - disease relative risk file.  Create lookup table of 
        /// relative risks as a function of disease status (diarrhea, anemia, stunting).  
        /// Report out to console any problems.
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool readFile03_diseaseRR(string fileName)
        {
            FileStream myFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(myFile);
            int lineNum = 2; // Keep track of where we are in the file in case there is a problem

            // Read in first two lines of file (because first line is column headings to be ignored).
            string inputRecord = fileReader.ReadLine(); inputRecord = fileReader.ReadLine();

            // Read until reaching the end of file.  Report errors and exit if any line has bad data
            while (inputRecord != null)
            {
                if (inputRecord != "")
                {
                    // Get disease state fields, create a Person object initialized with this
                    // set of diseases.
                    Person myPerson = DiseaseRR.DiseaseRRTablePerson(lineNum, inputRecord);
                    if (!myPerson.getInitalizeOK())
                    {
                        Console.WriteLine("File name was: {0}.", fileName);
                        Console.WriteLine("Hit any key"); Console.ReadKey(); return false;
                    }

                    // Get relative risk, create a double initialized with this value.
                    double RR = DiseaseRR.DiseaseRRTableRRValue(lineNum, inputRecord);
                    if (RR < 0)
                    {
                        Console.WriteLine("Hit any key"); Console.ReadKey(); return false;
                    }

                    if (DiseaseRR.diseaseRRTable.ContainsKey(myPerson))
                    {
                        Console.WriteLine("Error - Relative risk for disease combination entered twice.");
                        Console.WriteLine("File name was: {0}.", fileName);
                        Console.WriteLine("Entry: {0}", inputRecord);
                        Console.WriteLine("");
                        Console.ReadKey();
                        return false;
                    }

                    // Add the Person object (key) and relative risk (lookup value) to the dictionary.
                    DiseaseRR.diseaseRRTable.Add(myPerson, RR);
                }
                inputRecord = fileReader.ReadLine();
                lineNum++;
            }

            // If diseaseRRTable is empty, return with error
            if (DiseaseRR.diseaseRRTable.Count() == 0)
            {
                Console.WriteLine("Error - File contains no data.");
                Console.WriteLine("File name was: {0}.", file03_DiseaseRR_name);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // No errors encountered - so return true
            return true;
        }

        
        
        /// <summary> ////////////////////////////////////////////////////////////////////////////////////
        /// readFile04_Parameters
        /// Loads default parameter values from the defaultFile into the Parms dictionary.  
        /// The defaultParms dictionary key is the name of the variable, and the value is a List,
        /// each entry of which is a List.  The bottom level List can contain any number of 
        /// elements.  It is up to code elsewhere in the simulation to correctly parse these entries.
        /// Note - input file lines that do not start with a LETTER are ignored - hence, treated as comments.
        /// </summary> ////////////////////////////////////////////////////////////////////////////////////
        static private bool readFile04_Parameters(string fileName)
        {
            FileStream myFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(myFile);
            int lineNum = 1; // Keep track of where we are in the file in case there is a problem

            // Read in first line of file.
            string inputRecord = fileReader.ReadLine();

            // If file is empty, return with error
            if (inputRecord == null)
            {
                Console.WriteLine("Error - File contains no data.");
                Console.WriteLine("File name was: {0}", fileName);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // Read until reaching the end of file.  Report errors and exit if any line has bad data
            while (inputRecord != null)
            {
                if (!Parms.addNewParm(inputRecord, lineNum)) return false;
                inputRecord = fileReader.ReadLine();
                lineNum++;
            }

            // No errors encountered - so return true
            return true;
        }


        /// <summary> //////////////////////////////////////////////////////////////////////////
        /// readFile05_policies
        /// Open and read in File05 - policy specification file.  Create lookup table of 
        /// policies that specify their impact (on either risk of disease or on mortality).
        /// </summary> /////////////////////////////////////////////////////////////////////////
        static private bool readFile05_policies(string fileName)
        {
            FileStream myFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(myFile);
            int lineNum = 2; // Keep track of where we are in the file in case there is a problem

            // Read in first two lines of file (because first line is column headings to be ignored).
            string inputRecord = fileReader.ReadLine(); inputRecord = fileReader.ReadLine();

            // Read until reaching the end of file.  Report errors and exit if any line has bad data
            while (inputRecord != null)
            {
                if (inputRecord != "")
                {
                    // Get disease state fields, create a Policy object initialized with this
                    // set of diseases.
                    Policy myPolicy = new Policy(inputRecord, lineNum);
                    if (!myPolicy.getInitalizeOK())
                    {
                        Console.WriteLine("File name was: {0}.", fileName);
                        Console.WriteLine("Hit any key"); Console.ReadKey(); return false;
                    }

                    // Add the Policy object (key) and relative risk (lookup value) to the policy list.
                    Policy.PolicyList.Add(myPolicy);
                }
                inputRecord = fileReader.ReadLine();
                lineNum++;
            }

            // If diseaseRRTable is empty, return with error
            if (Policy.PolicyList.Count() == 0)
            {
                Console.WriteLine("Error - File contains no data.");
                Console.WriteLine("File name was: {0}.", fileName);
                Console.WriteLine("");
                Console.ReadKey();
                return false;
            }

            // No errors encountered - so return true
            return true;
        }
    }
}


