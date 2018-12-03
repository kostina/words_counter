# WordsCounter

Programm reads text file specified in appsettings.json. Words are considered separated depending on characters specified in appsettings.json file (`SeparatingChars` parameter). 

Depending on file size it will be processed in memory, or read line by like from hard drive (depends on a parameter `MaxSizeInMemoryKb` in appsettings.json).

Programm differentiates plural and singular forms, for example "cat" and "cat" will be counted as two different words. As long as "cat's" and "cat" is counted as two different words.

To run the application specify parameter `FilePath` in appsettings.json
