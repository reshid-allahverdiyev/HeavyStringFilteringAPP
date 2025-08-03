using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavyStringFilteringAPP.Core.Interfaces;

public interface ISimilarityService
{
    List<string> Apply(List<string> words);
}
