using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using WordsCounter.Counter;

[assembly: InternalsVisibleTo("WordsCounter.Tests")]
namespace WordsCounter.Processor
{
    internal interface IFileProcessor
    {
        Dictionary<string, long> ProcessFile();
    }

    /// <summary>
    /// Class which takes care of reading file. Can process file by loading it in memrory or reading line by line from disk depending on settings.
    /// </summary>
    internal class FileProcessor : IFileProcessor
    {
        private FileProcessorConfig _config;
        private ICounter _counter;
        private ILogger _logger;

        public FileProcessor(IOptions<FileProcessorConfig> config, ICounter counter, ILogger<FileProcessor> logger)
        {
            config = config ?? throw new ArgumentNullException(nameof(config));
            _config = config.Value;

            _counter = counter ?? throw new ArgumentNullException(nameof(counter));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Dictionary<string, long> ProcessFile()
        {
            try
            {
                var fileSize = GetFileSizeBytes();

                Dictionary<string, long> result;

                _logger.Log(LogLevel.Information, string.Format("File size is {0} KB {1}", fileSize / 1024, Environment.NewLine));

                var maxSizeInMemoryKB = _config.MaxSizeInMemoryKb;
                if (fileSize < maxSizeInMemoryKB)
                {
                    _logger.Log(LogLevel.Information, string.Format("Processing file in memory {0}", Environment.NewLine));
                    result = ProcessInMemory();
                }
                else
                {
                    _logger.Log(LogLevel.Information, string.Format("Processing file by each line", Environment.NewLine));
                    result = ProcessFileByLine();
                }

                return result;
            }
            catch (FileNotFoundException exception)
            {
                throw exception;
            }
        }

        double GetFileSizeBytes()
        {
            return new FileInfo(_config.FilePath).Length;
        }

        Dictionary<string, long> ProcessInMemory()
        {
            var lines = File.ReadLines(_config.FilePath);
            foreach (var line in lines)
            {
                _counter.ProcessLine(line);
            }

            return _counter.WordsOccurencies;
        }

        Dictionary<string, long> ProcessFileByLine()
        {
            int counter = 0;
            string line;

            using (var file = new StreamReader(_config.FilePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    _counter.ProcessLine(line);
                    counter++;
                }
            }

            return _counter.WordsOccurencies;
        }
    }
}
