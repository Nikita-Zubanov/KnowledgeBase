using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.ViewModel
{
    public class ReasoningViewModel
    {
        public IDictionary<int, IList<LinguisticVariable>> Reasoning { get; }

        public ReasoningViewModel(IDictionary<int, IList<LinguisticVariable>> reasoning)
        {
            Reasoning = reasoning;
        }
    }
}
