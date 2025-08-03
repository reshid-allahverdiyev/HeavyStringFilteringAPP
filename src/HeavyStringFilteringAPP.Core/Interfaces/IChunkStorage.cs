using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavyStringFilteringAPP.Core.Interfaces;

public interface IChunkStorage
{
    void AddChunk(string uploadId, int chunkIndex, string chunk);
    bool IsComplete(string uploadId);
    string CombineChunks(string uploadId);
    void RemoveUpload(string uploadId);
}
