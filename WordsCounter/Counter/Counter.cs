using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("Counter.Tests")]
namespace WordsCounter.Counter
{
    /// <summary>
    /// Class which can process string based on separating characters. Stores result of processing in Dictionary in memory.
    /// </summary>
    internal class Counter : ICounter
    {
        public Dictionary<string, long> WordsOccurencies { get; }

        private CounterConfig _config;

        public Counter(IOptions<CounterConfig> config)
        {
            if (config==null)
                throw new ArgumentNullException(nameof(config));

            config.Value.Verify();
            _config = config.Value;

            WordsOccurencies = new Dictionary<string, long>();
        }

        public void ProcessLine(string line)
        {
            string[] words = line.Split(_config.SeparatingChars, StringSplitOptions.RemoveEmptyEntries);

            CountWords(words);
        }

        void CountWords(string[] words)
        {
            foreach (var word in words)
            {
                var lowerCasedKey = word.ToLower();

                if (WordsOccurencies.ContainsKey(lowerCasedKey))
                {
                    var count = WordsOccurencies[lowerCasedKey];
                    WordsOccurencies[lowerCasedKey] = count + 1;
                }
                else
                {
                    WordsOccurencies[lowerCasedKey] = 1;
                }
            }
        }
    }
}
