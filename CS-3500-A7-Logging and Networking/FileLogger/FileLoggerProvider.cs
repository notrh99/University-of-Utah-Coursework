using Microsoft.Extensions.Logging;

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
    /// File Logger Provider that Wraps the FileLogger class.
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Keeps a reference to the FileLogger to dispose of when this object is disposed.
        /// </summary>
        private FileLogger? _fileLogger;


        /// <summary>
        /// Disposes of the FileLogger, closing the connection to the file.
        /// </summary>
        public void Dispose()
        {
            _fileLogger?.Dispose();
        }


        /// <summary>
        /// Creates a new Logger, and returns it.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            _fileLogger = new FileLogger(categoryName);

            return _fileLogger;
        }
    }
}
