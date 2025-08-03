using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavyStringFilteringAPP.Application.Requests;
using HeavyStringFilteringAPP.Core.Interfaces;
using HeavyStringFilteringAPP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeavyStringFilteringAPP.API.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class UploadChunksController : ControllerBase
{
    private readonly IChunkStorage _storage;
    private readonly FilteringQueueService _queue;

    public UploadChunksController(IChunkStorage storage, FilteringQueueService queue)
    {
        _storage = storage;
        _queue = queue;
    }

    [HttpPost]
    public IActionResult Upload([FromBody] UploadChunkRequest request)
    {
        _storage.AddChunk(request.UploadId, request.ChunkIndex, request.Data);

        if (request.IsLastChunk)
        {
            var fullText = _storage.CombineChunks(request.UploadId);
            _storage.RemoveUpload(request.UploadId);
            _queue.Enqueue(fullText);
        }

        return Accepted(new { status = "Accepted" });
    }
}
