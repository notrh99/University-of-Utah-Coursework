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
    /// Class that handles Opening/Creating a Log FIle, Recording to the Log File, and then Closing the log file when disposed.
    /// </summary>
    public class FileLogger : ILogger, IDisposable
    {
        /// <summary>
        /// Store the Filestream to dispose of correctly.
        /// </summary>
        private readonly FileStream _fileStream;

        /// <summary>
        /// Creates a Filestream to a Log file, persisting the connection until the object is disposed.
        /// If the log file does not exist, one is created.
        /// </summary>
        /// <param name="categoryName"></param>
        public FileLogger(string categoryName)
        {
            _fileStream = File.Open($"Log_CS3500_{categoryName}_Assignment7", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Disposes of the FileStream, to Close the connection to the File. 
        /// </summary>
        public void Dispose()
        {
            _fileStream.Dispose();
        }

        /// <summary>
        /// Logs the Formatted Line to the FIle, using a StreamWriter.
        ///
        /// Prepends the Thread Id and the DateTime to the log.
        ///
        /// Formats {LogLevel} - {formatter}.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_fileStream)
            {
                string logLine = $"{logLevel} - {formatter(state, exception)}";

                using (var streamWriter = new StreamWriter(_fileStream, leaveOpen: true))
                {
                    streamWriter.WriteLine(logLine.ShowTimeAndThread());
                }
            }
        }

        /// <summary>
        /// No comments neccessary as per assignment specifications.
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Associates the Log Files to eachother using a scope parameter - This Allows log files to be linked together via
        /// a key provided.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
