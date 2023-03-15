/// <summary> 
/// Author:    Iris Yang and Rayyan Hamid 
/// Partner:   None 
/// Date:      01/Mar/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Iris Yang and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Iris Yang and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>
/// 

using SpreadsheetGrid_Core;
using SpreadsheetUtilities;
using SS;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI
{
    public partial class SpreadsheetGUI : Form
    {
        Spreadsheet sheet;
        private bool overwrited;

        /// <summary>
        /// Zero-argument constructor
        /// Create new spreadsheet 
        /// </summary>
        public SpreadsheetGUI()
        {
            // Create new spreadhseet with vaild cell name and version of "six"
            sheet = new Spreadsheet(x => Regex.IsMatch(x, @"[a-zA-Z][1-9][0-9]?$"), x => x.ToUpper(), "six");
            InitializeComponent();
            spreadsheetGridWidget1.SelectionChanged += DisplaySelection;
            
            // Textbox which user can put the content
            ActiveControl = contentTextBox;
            overwrited = false;
        }

        /// <summary>
        /// Constructor to create spreadsheet GUI based on given file.
        /// </summary>
        /// <param name="filePath">The file path of file</param>
        public SpreadsheetGUI(String filePath)
        {
            // Create a new spreadsheet based on given file path
            try
            {
                sheet = new Spreadsheet(filePath, x => Regex.IsMatch(x, @"[a-zA-Z][1-9][0-9]?$"), x => x.ToUpper(), "six");
            }
            catch (SpreadsheetReadWriteException ex)
            {
                // Error to opening file
                MessageBox.Show("Can't open file!");
                return;
            }

            InitializeComponent();
            this.Text = filePath;

            spreadsheetGridWidget1.SelectionChanged += DisplaySelection;
            
            // Read given file and add cells on this spreadsheet
            List<String> cells = sheet.GetNamesOfAllNonemptyCells().ToList();
            foreach (string s in cells)
            {
                char colLetter = s[0];
                int col = colLetter - 'A';
                int.TryParse(s.Substring(1, s.Length - 1), out int row);
                row = row - 1;
                String temp = sheet.GetCellValue(s).ToString();
                spreadsheetGridWidget1.SetValue(col, row, sheet.GetCellValue(s).ToString());
            }

            ActiveControl = contentTextBox;
            overwrited = false;
        }

        /// <summary>
        /// Given a spreadsheet, find the current selected cell and
        /// create a popup that contains the information from that cell
        /// </summary>
        /// <param name="ss">SpreadsheetGridWidget</param>
        private void DisplaySelection(SpreadsheetGridWidget ss)
        {
            ActiveControl = contentTextBox;
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            String cellName = "" + (char)('A' + col) + (row + 1);
            nameTextBox.Text = cellName;
            valueTextBox.Text = value;
            contentTextBox.Text = value;
            if (sheet.GetNamesOfAllNonemptyCells().Contains(cellName))
            {
                if (sheet.GetCellContents(cellName) is Formula)
                    contentTextBox.Text = "=" + sheet.GetCellContents(cellName).ToString();
                else
                    contentTextBox.Text = sheet.GetCellContents(cellName).ToString();
            }
        }

        /// <summary>
        /// Called when new button clicked. Create a new spreadsheet
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetWindowApplicationContext.getAppContext().RunForm(new SpreadsheetGUI());
        }

        /// <summary>
        /// Called when save button clicked. Save a spreasheet
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (overwrited == true)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to overwrite on file?", "Error dialog?", MessageBoxButtons.YesNo);
                    // User say save
                    if (result == DialogResult.No)
                        return;
                }
                // Creates an SaveFileDialog let user to save the current spreadsheet
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save spreadsheet";
                saveFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
                saveFileDialog1.ShowDialog();

                // Save
                sheet.Save(saveFileDialog1.FileName);
            }

            catch (Exception ex)
            {
                // Any error happend while saving file
                MessageBox.Show("Error saving file", "Error");
                return;
            }

            
        }

        /// <summary>
        /// Called when open button clicked. Open a spreasheet
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Creates an OpenFileDialog that allows user to choose to open spreadsheet files or all files in dialog
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Open spreadsheet";
                openFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
                openFileDialog1.ShowDialog();

                // Open selected file on new window
                SpreadsheetWindowApplicationContext.getAppContext().RunForm(new SpreadsheetGUI(openFileDialog1.FileName));
               
            }

            catch (Exception ex)
            {
                // Any error happening to open file
                MessageBox.Show("Error opening file", "Error");
                return;
            }
        }

        /// <summary>
        /// Called when close button clicked. Close a spreasheet. 
        /// If there is unsaved data, or spreadsheet has changed from last saved version, then warn user with message box
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If there is unsaved changes, pop up message box to user
            if (sheet.Changed)
            {
                DialogResult result = MessageBox.Show("There is unsaved data in this file.", "Are you sure to close this file?", MessageBoxButtons.YesNo);
                // User say close
                if (result == DialogResult.Yes)
                    Close();
                // User say don't close
                else if (result == DialogResult.No)
                    return;
            }
            Close();
        }

        /// <summary>
        /// Called when the user presses a key while the CellContentsTextBox is focused
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void CellContentsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Possible key pressed are enter, down, up, left or right
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
                int row, col;
                spreadsheetGridWidget1.GetSelection(out col, out row);

                // If user add put anything on the contentTextBox
                if (contentTextBox.Text.Trim() != "")
                {
                    String cellName = "" + (char)('A' + col) + (row + 1);

                    if (sheet.GetCellContents(cellName) != "")
                    {
                        overwrited = true;
                    }

                    List<String> recalc = null;
                    try
                    {
                        // Set and grab list of things to re-calculate
                        recalc = new List<String>(sheet.SetContentsOfCell(cellName, contentTextBox.Text));
                    }
                    catch (CircularException ex)
                    {
                        // Circular exception among cells
                        MessageBox.Show("Error for circular exception " + cellName);
                        return;
                    }
                    catch (FormulaFormatException ex)
                    {
                        // Formula error on new added contentTextBox
                        MessageBox.Show("Error for wrong formula form " + cellName);
                        return;
                    }

                    //Display the recalculated cells 
                    foreach (string s in recalc)
                    {
                        char colLetter = s[0];
                        int col2 = colLetter - 'A';
                        int.TryParse(s.Substring(1, s.Length - 1), out int row2);
                        row2 = row2 - 1;
                        String temp = sheet.GetCellValue(s).ToString();
                        spreadsheetGridWidget1.SetValue(col2, row2, sheet.GetCellValue(s).ToString());
                    }
                    // Set 
                    spreadsheetGridWidget1.SetValue(col, row, sheet.GetCellValue(cellName).ToString());
                }

                // Move to other cell depends on key pressed
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        spreadsheetGridWidget1.SetSelection(col, row + 1);
                        DisplaySelection(spreadsheetGridWidget1);
                        break;
                    case Keys.Down:
                        spreadsheetGridWidget1.SetSelection(col, row + 1);
                        DisplaySelection(spreadsheetGridWidget1);
                        break;
                    case Keys.Up:
                        spreadsheetGridWidget1.SetSelection(col, row - 1);
                        DisplaySelection(spreadsheetGridWidget1);
                        break;
                    case Keys.Left:
                        spreadsheetGridWidget1.SetSelection(col - 1, row);
                        DisplaySelection(spreadsheetGridWidget1);
                        break;
                    case Keys.Right:
                        spreadsheetGridWidget1.SetSelection(col + 1, row);
                        DisplaySelection(spreadsheetGridWidget1);
                        break;
                }
            }
        }



        /// <summary>
        /// Called when help button clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Use mouse or keyboard to pick cell.\nYou can press enter, up, down, right, and left." +
                "\nPlease press those keys to save entered value on cell's content");
        }


        /// <summary>
        /// Special feature. Change background color of spreasheet window by random generated color
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            DialogResult result = MessageBox.Show("Do you like this background color?", "Color", MessageBoxButtons.YesNo);
            
            // Chnage color randomly
            if (result == DialogResult.No)
                spreadsheetGridWidget1.BackColor = Color.FromArgb(rand.Next(254), rand.Next(254), rand.Next(254));
            // do not change
            else if (result == DialogResult.Yes)
                return;

        }
    }
}