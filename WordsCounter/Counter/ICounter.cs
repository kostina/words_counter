using System.Collections.Generic;

namespace WordsCounter.Counter
{
    internal interface ICounter
    {
        Dictionary<string, long> WordsOccurencies { get; }
        void ProcessLine(string line);
    }
}