using System;
using System.Linq;

namespace WordsCounter.Counter
{
    internal class CounterConfig
    {
        public string[] SeparatingChars { get; set; }

        public void Verify()
        {
            if (SeparatingChars == null || !SeparatingChars.Any())
            {
                throw new ArgumentNullException(nameof(SeparatingChars));
            }
        }
    }
}
