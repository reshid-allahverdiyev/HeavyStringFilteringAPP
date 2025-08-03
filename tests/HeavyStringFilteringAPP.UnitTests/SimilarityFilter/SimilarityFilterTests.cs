using HeavyStringFilteringAPP.Infrastructure.Services;
using HeavyStringFilteringAPP.Application;
using Moq;

namespace HeavyStringFilteringAPP.UnitTests.LevenshteinFilter;

public class SimilarityFilterTests
{
    private readonly SimilarityService _similarityService;
    public SimilarityFilterTests()
    {
        var configService = new FilteringConfigService(
        0.60,
        0.80,
        "Levenshtein",
        new List<string> { "hlelo", "wordl" }
        );
        _similarityService = new SimilarityService(configService);
    }

    [Fact]
    public void ApplyFilter()
    {

        var result = _similarityService.Apply("hello world test lorem ipsum".Split(' ').ToList());

        Assert.DoesNotContain("hello", result);
        Assert.DoesNotContain("world", result);
        Assert.Contains("lorem", result);
        Assert.Contains("ipsum", result);
    }
}

