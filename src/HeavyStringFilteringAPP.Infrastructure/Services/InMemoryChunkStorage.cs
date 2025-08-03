using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavyStringFilteringAPP.Core.Interfaces;

namespace HeavyStringFilteringAPP.Infrastructure.Services;

public class InMemoryChunkStorage : IChunkStorage
{
    private readonly ConcurrentDictionary<string, SortedDictionary<int, string>> _storage = new();

    public void AddChunk(string uploadId, int chunkIndex, string chunk)
    {
        var list = _storage.GetOrAdd(uploadId, _ => new SortedDictionary<int, string>());
        lock (list)
        {
            list[chunkIndex] = chunk;
        }
    }

    public bool IsComplete(string uploadId) => _storage.ContainsKey(uploadId);

    public string CombineChunks(string uploadId)
    {
        if (!_storage.TryGetValue(uploadId, out var chunks)) return string.Empty;
        return string.Join("", chunks.OrderBy(x => x.Key).Select(x => x.Value));
    }

    public void RemoveUpload(string uploadId)
    {
        _storage.TryRemove(uploadId, out _);
    }
}
