using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavyStringFilteringAPP.Core.Interfaces;

public interface IFilteringConfigService
{
    double SimilarityThresholdLevenshtein { get; }
    double SimilarityThresholdJaroWinkler { get; }
    string Algorithm { get; }
    List<string> BlackListWords { get; }
}
