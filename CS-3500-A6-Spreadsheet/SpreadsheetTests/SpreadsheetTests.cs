/// <summary> 
/// Author:    Rayyan Hamid 
/// Partner:   None 
/// Date:      11/02/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>
///       

using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.Linq;

namespace GradingTests
{


    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {

        /// <summary>
        /// Testing GetCellContents with a null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        /// <summary>
        /// Testing GetCellContents with an invalid variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        /// <summary>
        /// Testing SetCellContent with null and empty string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, string.Empty);
        }


    }
}