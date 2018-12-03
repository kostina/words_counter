using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordsCounter.Tests")]
namespace WordsCounter.Processor
{
    internal class FileProcessorConfig
    {
        public int MaxSizeInMemoryKb { get; set; }
        public string FilePath { get; set; }

        public void Verify()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw  new ArgumentNullException(nameof(FilePath));
        }
    }
}
