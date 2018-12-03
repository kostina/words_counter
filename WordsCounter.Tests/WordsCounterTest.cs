using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using WordsCounter.Processor;
using Xunit;

namespace WordsCounter.Counter.Tests
{
    public class WordsCounterTest
    {
        [Fact]
        public void TestFileProcessor()
        {
            var fileProcessorConfig = new FileProcessorConfig() { FilePath = string.Format("{0}\\Test.txt", Environment.CurrentDirectory), MaxSizeInMemoryKb = 1 };
            var fileProcessorOptions = Options.Create(fileProcessorConfig);

            var counterConfig = new CounterConfig()
            {
                SeparatingChars = new[] {
                " ",
                ".",
                ",",
                ":",
                ";",
                "-",
                "\n",
                "\r\n",
                "\r"}
            };
            var counterOptions = Options.Create(counterConfig);

            var fileProcessor = new FileProcessor(fileProcessorOptions, new Counter(counterOptions), new Logger<FileProcessor>(new LoggerFactory()));
            var result = fileProcessor.ProcessFile();

            Assert.True(result["lorem"] == 2);
            Assert.True(result["ipsum"] == 2);
            Assert.True(result["is"] == 1);
            Assert.True(result["simply"] == 1);
            Assert.True(result["dummy"] == 2);
            Assert.True(result["text"] == 2);
            Assert.True(result["of"] == 2);
            Assert.True(result["the"] == 3);
            Assert.True(result["printing"] == 1);
            Assert.True(result["and"] == 2);
            Assert.True(result["typesetting"] == 1);
            Assert.True(result["industry"] == 1);
            Assert.True(result["has"] == 1);
            Assert.True(result["been"] == 1);
            Assert.True(result["industry's"] == 1);
            Assert.True(result["standard"] == 1);
            Assert.True(result["ever"] == 1);
            Assert.True(result["since"] == 1);
            Assert.True(result["1500s"] == 1);
            Assert.True(result["when"] == 1);
            Assert.True(result["an"] == 1);
            Assert.True(result["unknown"] == 1);
            Assert.True(result["printer"] == 1);
            Assert.True(result["took"] == 1);
            Assert.True(result["a"] == 2);
            Assert.True(result["galley"] == 1);
            Assert.True(result["type"] == 2);
            Assert.True(result["scrambled"] == 1);
            Assert.True(result["it"] == 1);
            Assert.True(result["to"] == 1);
            Assert.True(result["make"] == 1);
            Assert.True(result["specimen"] == 1);
            Assert.True(result["book"] == 1);
        }
    }
}
