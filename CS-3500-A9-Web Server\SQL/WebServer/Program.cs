using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebServer.Services;

namespace WebServer;

/// <summary> 
/// Author:    Tyler DeBruin
/// Partner:   Rayyan Hamid
/// Date:      4-27-2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
///
/// REntry Point for the App -Utilizes the DI Framework to Add Logging, the Secrets, and Register the Repository class.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point for the application. Keeps the WebService alive.
    /// </summary>
    static void Main()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var webServer = serviceProvider.GetRequiredService<Services.WebServer>();

        webServer.StartServer(11001);

        Console.WriteLine("WebServer listening on Port: 11001");
        Console.WriteLine("Press any key to shut the server down...");
        Console.ReadLine();

        webServer.StopServer();
    }

    /// <summary>
    /// Configures Logging, Secrets Configuration.
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureServices(ServiceCollection services)
    {
        var builder = new ConfigurationBuilder();
        builder.AddUserSecrets<Program>();

        IConfigurationRoot Configuration = builder.Build();

        var connectionString = new SqlConnectionStringBuilder
        {
            DataSource = Configuration["ServerURL"],
            InitialCatalog = Configuration["DBName"],
            UserID = Configuration["UserName"],
            Password = Configuration["DBPassword"],
            ConnectTimeout = 15
        }.ConnectionString;

        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.AddDebug();
            configure.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddScoped<Services.WebServer>();
        services.AddScoped<Repository>((x) => new Repository(connectionString));
    }

}