using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavyStringFilteringAPP.Application;
using HeavyStringFilteringAPP.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace HeavyStringFilteringAPP.Infrastructure.Services;

public class  SimilarityService : ISimilarityService
{
    private readonly IFilteringConfigService _config;
    public SimilarityService(IFilteringConfigService config)
    {
        _config = config;
    }

    public List<string> Apply(List<string> words)
    {
        if (_config.Algorithm == "Levenshtein")
            return words.Where(word =>
                !_config.BlackListWords.Any(filter =>
                    LevenshteinSimilarity(word, filter) >= _config.SimilarityThresholdLevenshtein))
                .ToList();
        else
            return words.Where(word =>
                 !_config.BlackListWords.Any(filter =>
                     JaroWinklerSimilarity(word, filter) >= _config.SimilarityThresholdJaroWinkler))
                 .ToList();
    }

    private double LevenshteinSimilarity(string s1, string s2)
    {
        int len1 = s1.Length, len2 = s2.Length;
        var d = new int[len1 + 1, len2 + 1];

        for (int i = 0; i <= len1; i++) d[i, 0] = i;
        for (int j = 0; j <= len2; j++) d[0, j] = j;

        for (int i = 1; i <= len1; i++)
            for (int j = 1; j <= len2; j++)
                d[i, j] = Math.Min(Math.Min(
                    d[i - 1, j] + 1,
                    d[i, j - 1] + 1),
                    d[i - 1, j - 1] + (s1[i - 1] == s2[j - 1] ? 0 : 1));

        int maxLen = Math.Max(len1, len2);
        return 1.0 - ((double)d[len1, len2] / maxLen);
    }

    public double JaroWinklerSimilarity(string s1, string s2)
    {
        if (s1 == s2) return 1.0;

        int len1 = s1.Length;
        int len2 = s2.Length;

        if (len1 == 0 || len2 == 0)
            return 0.0;

        int matchDistance = Math.Max(len1, len2) / 2 - 1;

        bool[] s1Matches = new bool[len1];
        bool[] s2Matches = new bool[len2];

        int matches = 0;
        int transpositions = 0;

        for (int i = 0; i < len1; i++)
        {
            int start = Math.Max(0, i - matchDistance);
            int end = Math.Min(i + matchDistance + 1, len2);

            for (int j = start; j < end; j++)
            {
                if (s2Matches[j] || s1[i] != s2[j])
                    continue;
                s1Matches[i] = true;
                s2Matches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0) return 0.0;

        int k = 0;
        for (int i = 0; i < len1; i++)
        {
            if (!s1Matches[i]) continue;
            while (!s2Matches[k]) k++;
            if (s1[i] != s2[k]) transpositions++;
            k++;
        }

        double jaro = (matches / (double)len1 + matches / (double)len2 + (matches - transpositions / 2.0) / matches) / 3.0;

        // Winkler bonus
        int prefix = 0;
        for (int i = 0; i < Math.Min(4, Math.Min(len1, len2)); i++)
        {
            if (s1[i] == s2[i])
                prefix++;
            else
                break;
        }

        return jaro + 0.1 * prefix * (1 - jaro);
    }
}
