using HeavyStringFilteringAPP.Infrastructure.Services;
using HeavyStringFilteringAPP.Application;
using Microsoft.Extensions.Logging.Abstractions;

namespace HeavyStringFilteringAPP.UnitTests.FilteringQueue;

public class UploadChunksTests
{
    private readonly FilteringQueueService _service;
    private readonly InMemoryChunkStorage _storage;
    public UploadChunksTests()
    {
        var configService = new FilteringConfigService(
       0.60,
       0.80,
       "Levenshtein",
       new List<string> { "hlelo", "wordl" }
       );

        var _similarityService = new SimilarityService(configService);
        var logger = NullLogger<FilteringQueueService>.Instance;
        _service = new FilteringQueueService(_similarityService, logger);
        _storage = new InMemoryChunkStorage();

    }


    [Fact]
    public async Task EnqueueWorks()
    {
        string baseDir = AppContext.BaseDirectory;
        string projectRoot = Directory.GetParent(baseDir)
            ?.Parent
            ?.Parent
            ?.Parent
            ?.Parent
            ?.Parent
            ?.FullName ?? throw new Exception("Cannot determine project root path.");

        string filePath = Path.Combine(projectRoot, "src\\assets", "basewords.txt");
        List<string> baseWords = File.ReadAllLines(filePath).ToList();

        string uploadId = Guid.NewGuid().ToString();
        int chunkSize = 100;
        int chunkIndex = 0;

        for (int i = 0; i < baseWords.Count; i += chunkSize)
        {
            var chunkWords = baseWords.Skip(i).Take(chunkSize);
            string combinedChunk = string.Join(" ", chunkWords);
            _storage.AddChunk(uploadId, chunkIndex++, combinedChunk);
        }
        var fullText = _storage.CombineChunks(uploadId);
        _storage.RemoveUpload(uploadId);
        _service.Enqueue(fullText);


        var cts = new CancellationTokenSource();
        var runTask = Task.Run(() => _service.StartAsync(cts.Token));

        await Task.Delay(10000);

        cts.Cancel();
        await runTask;

        Assert.True(true);
    }
}

