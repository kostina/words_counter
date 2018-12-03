using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WordsCounter.Counter;
using WordsCounter.Processor;

namespace WordsCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = ConfigureServices();
                var serviceProvider = SetupServices(config);

                var logger = serviceProvider
                    .GetRequiredService<ILogger<Program>>();

                var fileProcessor = serviceProvider.GetService<IFileProcessor>();
                var result = fileProcessor.ProcessFile();

                using (logger.BeginScope("File processed. Result:"))
                {
                    foreach (var item in result)
                    {
                        logger.Log(LogLevel.Information,
                            string.Format("Count: {0} : Word : {1}", item.Value, item.Key));
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(string.Format("File not found, please specify correct path in appsettings.json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception occured during program execution : {0} {1} {2} {3} {4}", ex.Message, Environment.NewLine, ex.InnerException, Environment.NewLine, ex.StackTrace));
            }

            Console.ReadLine();
        }

        static IConfiguration ConfigureServices()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        static ServiceProvider SetupServices(IConfiguration config)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<FileProcessorConfig>(config.GetSection(nameof(FileProcessorConfig)));
            services.Configure<CounterConfig>(config.GetSection(nameof(CounterConfig)));

            services
                .AddScoped<IFileProcessor, FileProcessor>()
                .AddSingleton<ICounter, Counter.Counter>()
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<Program>();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Debug));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
