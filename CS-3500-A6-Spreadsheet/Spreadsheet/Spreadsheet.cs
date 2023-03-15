
/// <summary> 
/// Author:    Iris Yang
/// Partner:   None
/// Date:      Feb/15/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Iris Yang - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Iris Yang, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
///    Construct internal of spreadsheet program with using Formula and DependencyGraph libraries. 
/// </summary>
/// 
/// Version 1.1 Assignment 4
/// Version 2.1 Modify and add more features for Assignment 5
/// 
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SS
{
    /// <summary>
    /// Class that construct internal of spreadsheet.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cellSpreadsheet;
        private DependencyGraph cellDependencyGraph;
        private bool changed;

        /// <summary>
        /// Empty constructor 
        /// Changed due to AS5
        /// </summary>
        public Spreadsheet() : base(v => true, n => n, "default")
        {
            cellSpreadsheet = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Three-argument constructor allows user to provide delegates
        /// New feature for AS5
        /// </summary>
        /// <param name="isValid">Validity delegate</param>
        /// <param name="norm">normalization delegate</param>
        /// <param name="version">version</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> norm, string version) : base(isValid, norm, version)
        {
            IsValid = isValid;
            Normalize = norm;
            cellSpreadsheet = new Dictionary<String, Cell>();
            cellDependencyGraph = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Four-argument constructor allows user to provide delegates and file path
        /// New feature for AS5
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="isValid">validity delegate</param>
        /// <param name="norm">Normalization delegate</param>
        /// <param name="version">Version</param>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            IsValid = isValid;
            Normalize = normalize;
            cellSpreadsheet = new Dictionary<String, Cell>();
            cellDependencyGraph = new DependencyGraph();
            changed = false;

            // Unmatched version
            if (!GetSavedVersion(filePath).Equals(version))
                throw new SpreadsheetReadWriteException("Version does not match");

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    string name = "";
                    string content = "";
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "name":
                                    reader.Read();
                                    name = reader.ReadContentAsString();
                                    name.Trim();
                                    break;

                                case "contents":
                                    reader.Read();
                                    content = reader.ReadContentAsString();
                                    content.Trim();
                                    SetContentsOfCell(name, content);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("Unexpected error while reading file");
            }
        }

        /// <summary>
        /// Keep track of modifying status of spreadsheet. 
        /// True if spreadsheet has been changed after saving or creating, otherwise false
        /// New feature for AS5
        /// </summary>
        public override bool Changed { get => changed; protected set => throw new NotImplementedException(); }

        /// <summary>
        /// <inheritdoc/>
        /// Return to content in given named cell
        /// </summary>
        /// <param name="name">Name of cell</param>
        /// <returns>Content in cell</returns>
        public override object GetCellContents(string name)
        {
            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(name);

            // Even cell does not assigned with specific value, it should have empty cell rather than non-existent cell
            if (!cellSpreadsheet.ContainsKey(Normalize(name)))
                return "";

            return cellSpreadsheet[Normalize(name)].getContent();

        }

        /// <summary>
        /// <inheritdoc/>
        /// If cell name is valid, return value of cell.
        /// New feature for AS5
        /// </summary>
        /// <param name="name">String name of cell</param>
        /// <returns>string, double or formula error which are content of cell</returns>
        /// <exception cref="InvalidNameException">If cell name is null or invalid</exception>
        public override object GetCellValue(string name)
        {
            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(Normalize(name));

            if (cellSpreadsheet.TryGetValue(Normalize(name), out Cell cell))
            {
                return cell.getContent();
            }
            else
                return "";
        }

        /// <summary>
        /// <inheritdoc/>
        /// The cell can be any types such as formula or double, etc.
        /// </summary>
        /// <returns>Names of non-empty cell</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> nonEmptyCellList = new List<string>();

            foreach (KeyValuePair<string, Cell> cell in cellSpreadsheet)
            {
                // Check cells and if they are not null, add into the list
                if (!ReferenceEquals(cell.Value.getContent(), string.Empty))
                    nonEmptyCellList.Add(cell.Key);
            }
            return nonEmptyCellList;
        }

        /// <summary>
        /// <inheritdoc/>
        /// New feature for AS5
        /// </summary>
        /// <param name="filename">Strign which is name of file</param>
        /// <returns></returns>
        /// <exception cref="SpreadsheetReadWriteException">When there are any problems</exception>
        public override string GetSavedVersion(string filename)
        {
            // Problem for opening the file
            if (ReferenceEquals(filename, null) || filename.Equals(""))
                throw new SpreadsheetReadWriteException("File name is invalid");
            try
            {
                using (XmlReader read = XmlReader.Create(filename))
                {
                    while (read.Read())
                    {
                        //Grab the correct tag and return the attribute
                        if (read.Name.Equals("spreadsheet"))
                            return read.GetAttribute("version");
                    }
                }
            }
            // Problem for reading or closing the file
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("While reading or closing the file, there is error");
            }
            return "default";
        }

        /// <summary>
        /// <inheritdoc/>
        /// New feature for AS5
        /// </summary>
        /// <param name="filename">String file name</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Save(string filename)
        {
            if (ReferenceEquals(filename, null) || filename.Equals(""))
                throw new SpreadsheetReadWriteException("File name cannot be null or empty");

            try
            {
                changed = false;
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    //Start the document and label the beginning
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", null, Version);

                    foreach (KeyValuePair<string, Cell> cell in cellSpreadsheet)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell.Key);

                        object cellEvaluatedContent = cell.Value.getEvaluate();

                        // when content is formula
                        if (cellEvaluatedContent is Formula)
                        {
                            Formula f = (Formula)cellEvaluatedContent;
                            writer.WriteElementString("contents", "=" + f.ToString());
                        }

                        // When content is double
                        else if (cellEvaluatedContent is double)
                        {
                            writer.WriteElementString("contents", cellEvaluatedContent.ToString());
                        }

                        // When content is string
                        else
                        {
                            writer.WriteElementString("contents", (string)cellEvaluatedContent);
                        }

                        writer.WriteEndElement();
                        continue;
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("Error occured while operating spreadsheet.");
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// Set content of named cell in spreadsheet and update dependency graph
        /// Changed to protected from AS4 for AS5
        /// </summary>
        /// <param name="name">String represents name of cell</param>
        /// <param name="number">Double which is cell's content</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(name);

            // This private method will modify Changed to true
            SetCellContentHelper(name, number);

            // Remove old dependees
            cellDependencyGraph.ReplaceDependees(name, new HashSet<string>());

            // Return to recalculated
            return new List<string>(new HashSet<string>(GetCellsToRecalculate(name)));
        }

        /// <summary>
        /// <inheritdoc/>
        /// Set content of named cell in spreadsheet and update dependency graph
        /// Changed to protected from public for AS5
        /// </summary>
        /// <param name="name">String represents name of cell</param>
        /// <param name="text">String which is cell's content</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(name);

            // This private method will modify Changed to true
            SetCellContentHelper(name, text);

            cellDependencyGraph.ReplaceDependents(name, new HashSet<string>());

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// <inheritdoc/>
        /// Set content of named cell in spreadsheet and update dependency graph
        /// Changed to protected from public for AS5
        /// </summary>
        /// <param name="name">String represents name of cell</param>
        /// <param name="formula">Formula which is cell's content</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {

            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(name);

            // Temporary store direct dependents of named cell for using if there is
            // circular exception throws
            IEnumerable<String> temp = cellDependencyGraph.GetDependees(name);

            // Update to new dependency
            cellDependencyGraph.ReplaceDependees(name, formula.GetVariables());

            try
            {
                List<String> current = new List<String>(GetCellsToRecalculate(name));
                // GetCellsToRecalculate method will check if cell has circular dependency
            }

            catch (CircularException)
            {
                // Back to original 
                cellDependencyGraph.ReplaceDependees(name, temp);
                throw new CircularException();
            }

            // This private method will modify Changed to true
            SetCellContentHelper(name, formula);

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// <inheritdoc/>
        /// New feature for AS5
        /// </summary>
        /// <param name="name">String cell name</param>
        /// <param name="content">String cell's content</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            // Check for either cell name is valid or not. If not, it will throw invalid name exception
            IsValidCellName(name);

            // Check if the content is a formula
            // Now content starts with equal sign "="
            if (content.StartsWith("="))
            {
                // SetCellContents, private method will modify Changed to true
                 List<String> formulaContent = new List<String>(SetCellContents(Normalize(name),
                    new Formula(content.Substring(1, content.Length - 1), Normalize, IsValid)));

                // If it content has cell which needs to re-evaluate, do it
                foreach (string cellName in formulaContent)
                {
                    if (cellSpreadsheet.TryGetValue(cellName, out Cell cell))
                        cell.Reevaluate(Lookup);
                }

                return formulaContent;
            }

            // Check if the content is a double
            else if (Double.TryParse(content, out double temp))
            {
                // SetCellContents, private method will modify Changed to true
                List<String> doubleContent = new List<String>(SetCellContents(Normalize(name), temp));

                // If it content has cell which needs to re-evaluate, do it
                foreach (string cellName in doubleContent)
                {
                    if (cellSpreadsheet.TryGetValue(cellName, out Cell cell))
                        cell.Reevaluate(Lookup);
                }

                return doubleContent;
            }

            // When content is string
            else
            {
                // SetCellContents, private method will modify Changed to true
                List<String> stringContent = new List<String>(SetCellContents(name, content));

                // If it content has cell which needs to re-evaluate, do it
                foreach (string cellName in stringContent)
                {
                    if (cellSpreadsheet.TryGetValue(cellName, out Cell cell))
                        cell.Reevaluate(Lookup);
                }

                return stringContent;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// Get all cells whose contents are directly depends on named cell
        /// </summary>
        /// <param name="name">String which is name of cell</param>
        /// <returns>Enumeration of dependents of named cell</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return cellDependencyGraph.GetDependents(name);
        }

        /// <summary>
        /// Private helper method to check either name of cell is valid or not
        /// </summary>
        /// <param name="name">String represents name of cell</param>
        /// <exception cref="InvalidNameException">If name of cell is invalid or null</exception>
        private void IsValidCellName(string name)
        {
            // When name of cell is null or invalid, throw invalid name exception
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !IsValid(name))
                throw new InvalidNameException();

        }

        /// <summary>
        /// Private helper method to set cell content on given named cell
        /// New feature for AS5 - keep track of changing of cell's content
        /// </summary>
        /// <param name="name">String which represents name of cell</param>
        /// <param name="content">Object which is content of named cell</param>
        private void SetCellContentHelper(string name, object content)
        {
            // Spreadsheet already contains cell
            if (cellSpreadsheet.ContainsKey(name))
            {
                // update content 
                cellSpreadsheet[name] = new Cell(name, content, Lookup);
                // tODO : update its evalueated content too


                // Cell's content gets changed
                changed = true;
            }
            else
            {
                cellSpreadsheet.Add(name, new Cell(name, content, Lookup));
                // Cell's content gets changed
                changed = true;
            }

        }

        /// <summary>
        /// Private helper method to get content of cell outside of private cell class
        /// </summary>
        /// <param name="s">String cell name</param>
        /// <returns>double cotnent of cell</returns>
        /// <exception cref="ArgumentException"></exception>
        private double Lookup(string name)
        {
            if (cellSpreadsheet.TryGetValue(name, out Cell temp))
            {
                return (double)temp.getContent();
            }

            else
                throw new ArgumentException("Can't get cell's content");
        }

        /// <summary>
        /// Private class to store each cell's own data
        /// </summary>
        private class Cell
        {
            private object content;
            private string name;
            private object evaluatedContent;

            /// <summary>
            /// Constructor for cell class 
            /// </summary>
            /// <param name="cellName"></param>
            /// <param name="cellContent"></param>
            public Cell(string cellName, object cellContent, Func<string, double> lookup)
            {
                evaluatedContent = cellContent;
                name = cellName;

                if (evaluatedContent is Formula)
                {
                    Formula contentFormula = (Formula)evaluatedContent;
                    content = contentFormula.Evaluate(lookup);
                }
                else
                    content = evaluatedContent;

            }

            /// <summary>
            /// Getter method for content of cell
            /// </summary>
            /// <returns>Content of cell</returns>
            public object getContent()
            {
                return content;
            }

            /// <summary>
            /// Setter method for content of cell
            /// </summary>
            /// <param name="newContent">Value to replace old content</param>
            public void SetContent(object newContent)
            {
                content = newContent;
            }

            /// <summary>
            /// Getter method for evaluated content of cell
            /// </summary>
            /// <returns>C ontent of cell</returns>
            public object getEvaluate()
            {
                return evaluatedContent;
            }

            /// <summary>
            /// Re-evaluate cell when its dependent cell has been updated or hav new dependent cell
            /// </summary>
            /// <param name="lookup"></param>
            public void Reevaluate(Func<string, double> lookup)
            {
                if (evaluatedContent is Formula)
                {
                    Formula contentFormula = (Formula)evaluatedContent;
                    content = contentFormula.Evaluate(lookup);
                }
            }
        }
    }
}