# HeavyStringFilteringAPP
Web API that accepts a very long text string in chunks, stores it, and sends the full text to a background queue. The filtering logic will run in the background using in-memory filtering rules .

# HeavyStringFilteringAPP (.NET 8 Clean Architecture)
This is  Clean Architecture Web API project built with .NET 8.

## ✅ Features

- .NET 8 Web API 
- Manual Dependency Injection
- Swagger enabled
- Unit tests with xUnit
- Layered structure: Core, Application, Infrastructure, API


## Initializing parameter gave on appsettings.json
 -  "AlgorithmDescription": "Replace Levenshtein or JaroWinkler  on Algoritm options",
 -  "Algorithm": "Levenshtein" or  "Algorithm": "JaroWinkler" ,
 -  By default  using  Levenshtein algorithm 
 

## Exacuting test cases
-  dotnet test tests/HeavyStringFilteringAPP.UnitTests
-  SimilarityFilterTests  -  tests both Algorithms
-  FilteringQueueServiceTests - tests queue works
-  UploadChunksTests - tests main endpoint using lagre text file located basewords.txt



## 📁 Project Structure
HeavyStringFilteringAPP/
├── src/
│ ├── HeavyStringFilteringAPP.API  
│ ├── HeavyStringFilteringAPP.Core  
│ ├── HeavyStringFilteringAPP.Application  
│ └── HeavyStringFilteringAPP.Infrastructure  
├── tests/
│ └── HeavyStringFilteringAPP.UnitTests 
├── HeavyStringFilteringAPP.sln
├── .gitignore
└── README.md
