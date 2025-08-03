using HeavyStringFilteringAPP.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HeavyStringFilteringAPP.Application;

public class FilteringConfigService : IFilteringConfigService
{
    public double SimilarityThresholdLevenshtein { get; }
    public double SimilarityThresholdJaroWinkler { get; }
    public string Algorithm { get; }
    public List<string> BlackListWords { get; }
    public FilteringConfigService(
           double similarityThresholdLevenshtein,
           double similarityThresholdJaroWinkler,
           string algorithm,
           List<string> blacklistWords)
    {
        SimilarityThresholdLevenshtein = similarityThresholdLevenshtein;
        SimilarityThresholdJaroWinkler = similarityThresholdJaroWinkler;
        Algorithm = algorithm;
        BlackListWords = blacklistWords;
    }
    public FilteringConfigService(IConfiguration configuration, IWebHostEnvironment env)
    {
        var section = configuration.GetSection("Filtering");

        var srcFolder = Directory.GetParent(env.ContentRootPath)?.Parent?.FullName ?? "";


        if (!double.TryParse(section["SimilarityThresholdLevenshtein"], out var lev))
            lev = 0.8;

        if (!double.TryParse(section["SimilarityThresholdJaroWinkler"], out var jaro))
            jaro = 0.85;

        SimilarityThresholdLevenshtein = lev;
        SimilarityThresholdJaroWinkler = jaro;
        Algorithm = section["Algorithm"] ?? "Levenshtein";

        var blackListPath = Path.Combine(srcFolder, section["BlackListPath"] ?? "");

        BlackListWords = File.Exists(blackListPath)
            ? File.ReadAllLines(blackListPath).ToList()
            : new List<string> { "test" };
    }
}