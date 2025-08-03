namespace HeavyStringFilteringAPP.Core.Interfaces;

public interface IQueueProcessor
{
    void Enqueue(string rawText);
}
