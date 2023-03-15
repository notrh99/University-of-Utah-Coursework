namespace FileLogger
{
    /// <summary> 
    /// Author:    Tyler DeBruin and Rayyan Hamid
    /// Partner:   None
    /// Date:      3/28/2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// String Extension method for Prepending the DateTime and the Thread Number to a string.
    /// </summary>
    public static class ShowTimeAndThreadClass
    {
        /// <summary>
        /// Prepends the DateTime, and the ThreadNumber to the start of the string.
        /// 2020-03-20 1:38:35 PM (1)
        /// </summary>
        /// <param name="value">String object using this method, I.e. 'This String'.</param>
        /// <returns>A string in the format: YYYY-MM-DD HH:MM:SS PM/AM (1) - {value}</returns>
        public static string ShowTimeAndThread(this string value)
        {
            return $"{DateTime.Now} {Thread.CurrentThread.ManagedThreadId} - {value}";
        }
    }
}
