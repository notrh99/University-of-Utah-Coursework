using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatClient
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
    /// Startup class for the ChatClient.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var gui = serviceProvider.GetRequiredService<ChatClient>();

            Application.Run(gui);
        }

        /// <summary>
        /// Configures Logging, and Adds the ChatServer.
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
                configure.SetMinimumLevel(LogLevel.Debug);
                configure.AddProvider(new FileLoggerProvider());
            });

            services.AddScoped<ChatClient>();
        }
    }
}