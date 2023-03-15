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

namespace GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            SpreadsheetWindowApplicationContext appContext = SpreadsheetWindowApplicationContext.getAppContext();
            appContext.RunForm(new SpreadsheetGUI());
            Application.Run(appContext);
        }
    }

    /// <summary>
    /// Class which deals of application context of spreadsheet GUI
    /// </summary>
    class SpreadsheetWindowApplicationContext : ApplicationContext
    {
        //Number of open forms
        private int formCount = 0;

        //Singleton ApplicationContext
        private static SpreadsheetWindowApplicationContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetWindowApplicationContext()
        {

        }

        /// <summary>
        /// Build GUI window
        /// </summary>
        /// <param name="form">Form that is GUI window</param>
        public void RunForm(Form form)
        {
            //One more form is running
            formCount++;

            // Event handler when GUI is closed
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            //Run the form
            form.Show();
        }

        /// <summary>
        /// Returns application context
        /// </summary>
        /// <returns>Application context</returns>
        public static SpreadsheetWindowApplicationContext getAppContext()
        {
            if (appContext == null)
                appContext = new SpreadsheetWindowApplicationContext();
            return appContext;
        }
    }
}