using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Modules
{
    internal class WorkingMemory
    {
        public IList<Judgment> Factors { get; private set; }

        public WorkingMemory(IList<Judgment> inputFactors)
        {
            Factors = inputFactors;
        }

        public void AddFactor(Judgment factor)
        {
            Factors.Add(factor);
        }
        public void AddRangeFactors(IList<Judgment> factors)
        {
            foreach (Judgment factor in factors)
                Factors.Add(factor);
        }

        public bool ContainsAny(IList<Judgment> judgments)
        {
            foreach (Judgment judgment in judgments)
                if (Factors.Contains(judgment))
                    return true;
            return false;
        }
    }
}
