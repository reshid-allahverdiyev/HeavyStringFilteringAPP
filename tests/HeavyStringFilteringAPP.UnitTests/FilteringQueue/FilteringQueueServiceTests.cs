using HeavyStringFilteringAPP.Infrastructure.Services;
using HeavyStringFilteringAPP.Application;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;
using HeavyStringFilteringAPP.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace HeavyStringFilteringAPP.UnitTests.FilteringQueue;


public class FilteringQueueServiceTests
{
    private readonly FilteringQueueService _service;

    public FilteringQueueServiceTests()
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

    }


    [Fact]
    public async Task EnqueueWorks()
    {
        _service.Enqueue("hello world test lorem ipsum");

        var cts = new CancellationTokenSource();
        var runTask = Task.Run(() => _service.StartAsync(cts.Token));

        await Task.Delay(500);

        cts.Cancel();
        await runTask;

        Assert.True(true);
    }
}

