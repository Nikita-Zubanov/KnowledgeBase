using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.InputOutputVariables
{
    public interface IParameter
    {
        FactorTitle Title { get; }
        string Description { get; }
        decimal? Value { get; }
        string Unit { get; }
    }
}
