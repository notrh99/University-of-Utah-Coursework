/// <summary> 
/// Author:    Rayyan Hamid 
/// Partner:   None 
/// Date:      28/01/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two 
    /// ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 
    /// equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an 
    /// element to a
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is
    ///called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is
    /// called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>

        // Making a new dictionary for dependents
        private Dictionary<string, HashSet<string>> setOfDependents;
        // Making a new dictionary for dependees
        private Dictionary<string, HashSet<string>> setOfDependees;
        public DependencyGraph()
        {
            setOfDependents = new Dictionary<string, HashSet<string>>();
            setOfDependees = new Dictionary<string, HashSet<string>>();

        }
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get; private set;

        }
        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you
        /// would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (setOfDependees.ContainsKey(s))
                    return setOfDependees[s].Count;
            else
                return 0;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (!(setOfDependents.ContainsKey(s)))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (!(setOfDependees.ContainsKey(s)))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (!HasDependents(s))
                return new HashSet<string>();
            else
                return new HashSet<string>(setOfDependents[s]);
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!HasDependees(s))
                return new HashSet<string>();
            else
                return new HashSet<string>(setOfDependees[s]);
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //If the given ordered pair is not in the Dependency Graph we will increment the count
            if (!(setOfDependents.ContainsKey(s) && setOfDependees.ContainsKey(t)))
            {
                Size++;
            }
            //If the setOfDependents contains s, but does not contain t.
            if ((setOfDependents.ContainsKey(s)) && !(setOfDependents[s].Contains(t)))
            {
                setOfDependents[s].Add(t);
            }
            //If the setOfDependents does not contain s.
            else if (!setOfDependents.ContainsKey(s))
            {
                //Creating a new Hashset
                HashSet<string> dees = new HashSet<string>();
                dees.Add(t);
                setOfDependents.Add(s, dees);

            }

            //If the setOfDependees only contains t.
            if (setOfDependees.ContainsKey(t))
            {
                setOfDependees[t].Add(s);

            }
            //If the setOfDependees does not contains t.
            else if (!(setOfDependees.ContainsKey(t)))
            {
                //Creating a new Hashset
                HashSet<string> dents = new HashSet<string>();
                dents.Add(s);
                setOfDependees.Add(t, dents);
            }
        }
        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //If the given ordered pair is in the dependency graph
            if ((setOfDependents.ContainsKey(s) && setOfDependees.ContainsKey(t)))
            {
                Size--;
            }
            //If the setOfDependents contains s.
            if (setOfDependents.ContainsKey(s))
            {
                setOfDependents[s].Remove(t);
                if (setOfDependents[s].Count == 0)
                    setOfDependents.Remove(s);
            }
            //If the setOfDependents contains t.
            if (setOfDependees.ContainsKey(t))
            {
                setOfDependees[t].Remove(s);
                if (setOfDependees[t].Count == 0)
                    setOfDependees.Remove(t);
            }

        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // Remove dependent 'r' also called s in the previousDependents
            foreach (string r in GetDependents(s))
            {
                RemoveDependency(s, r);
            }
            // Add dependent 't' also called s in newDependents
            foreach (string t in newDependents)
            {
                AddDependency(s, t);

            }
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // Remove dependee 'r' also called s in the previousDependents
            foreach (string r in GetDependees(s))
            {
                RemoveDependency(r, s);
            }
            // Add dependee 't' also called s in newDependents
            foreach (string t in newDependees)
            {
                AddDependency(t, s);

            }
        }
    }
}