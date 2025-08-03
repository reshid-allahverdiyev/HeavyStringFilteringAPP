using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavyStringFilteringAPP.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HeavyStringFilteringAPP.Infrastructure.Services;

public class FilteringQueueService : BackgroundService
{
    private readonly ConcurrentQueue<string> _queue = new();
    private readonly ISimilarityService _similarityService;
    private readonly ILogger<FilteringQueueService> _logger;

    public FilteringQueueService(ISimilarityService similarityService, ILogger<FilteringQueueService> logger)
    {
        _similarityService = similarityService;
        _logger = logger;
    }

    public void Enqueue(string text) => _queue.Enqueue(text);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            {
                if (_queue.TryDequeue(out var text))
                {
                    var words = text.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).ToList();
                    var filtered = _similarityService.Apply(words);
                    _logger.LogInformation("Filtered Words: {Count}", filtered.Count);
                }
                await Task.Delay(10, stoppingToken);
            }
        }
    }
}
