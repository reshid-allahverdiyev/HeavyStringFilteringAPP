using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavyStringFilteringAPP.Application.Requests;

public class UploadChunkRequest
{
    public string UploadId { get; set; } = string.Empty;
    public int ChunkIndex { get; set; }
    public string Data { get; set; } = string.Empty;
    public bool IsLastChunk { get; set; }
}
