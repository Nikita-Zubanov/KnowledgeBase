using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.FundamentalVariables
{
    public interface ILinguisticVariable
    {
        FactorTitle Title { get; }
        FactorFuzzyValue FuzzyValue { get; }
        decimal? Value { get; }

        Dictionary<FactorFuzzyValue, decimal[]> ValueRanges { get; }
        IList<FactorFuzzyValue> TermSet { get; }

        string Description { get; }
        string Unit { get; }
    }
}
