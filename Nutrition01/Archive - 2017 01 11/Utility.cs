using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL1
{
    class Utility
    {
        /*
        //****************************************************************************************
        //****************************************************************************************
        // UTILITY FUNCTIONS
        //****************************************************************************************
        //****************************************************************************************
        
        //****************************************************************************************
        // Takes a probability and returns a rate
        //****************************************************************************************
        public static double rateFromProb(double p)
        {
            double r = -Math.Log(1 - p);
            return r;
        }

        //****************************************************************************************
        // Takes a rate and returns a probability
        //****************************************************************************************
        public static double probFromRate(double r)
        {
            double p = 1 - Math.Exp(-r);
            return p;
        }

        //****************************************************************************************
        // Computes discounted QALYs accrued as a function of timestep, utility weight, and time 
        // since start of simulation.
        //****************************************************************************************
        public static double deltaQALY(double timestep, double util, double timeSinceStart)
        {
            double t = (0.5 * timestep) + timeSinceStart;
            double dr = K.discountRate;
            double b = 1 + dr;
            double denom = Math.Pow(b,t);
            double q = timestep * util * (1 /denom);
            return q;
        }

        //****************************************************************************************
        // Computes a transition probability for specified timeStep given a vector containing 
        // two elements - a probability over a duration, and the duration.  
        //****************************************************************************************
        public static double getProb(double[] a, double timeStep)
        {
            double prob = a[0];
            double years = a[1];
            double r;
            double p;
            r = Utility.rateFromProb(prob);
            r = r * (1 / years);
            r = (1 / timeStep) * r;
            p = Utility.probFromRate(r);
            return p;
        }

        //****************************************************************************************
        // Gets background mortality rate as a function of age and the timestep duration, 
        // scaling the corresponding mortality rate by the specified SMR. 
        //****************************************************************************************
        public static double getBackgroundMortProb(double myAge, double timeStep, double SMR)
        {
            double returnVal = 0;
            double mortProbUnadjusted = getBackgroundMortProb(myAge, timeStep);
            double mortRate = SMR * Utility.rateFromProb(mortProbUnadjusted);
            returnVal = Utility.probFromRate(mortRate);
            return returnVal;
        }

        //****************************************************************************************
        // Gets background mortality rate as a function of age and the timestep duration. 
        //****************************************************************************************
        public static double getBackgroundMortProb(double myAge, double timeStep)
        {
            double returnVal = 0;
            int index = (int)myAge;
            index = Math.Min(index, (K.backgroundNeutralDeathProb.Count() - 1));
            double prob = K.backgroundNeutralDeathProb[index];
            if (prob == 1) return prob;
            double r = Utility.rateFromProb(prob);
            r = r * (1 / timeStep);
            returnVal = Utility.probFromRate(r);
            return returnVal;
        }
        */
    }
}
