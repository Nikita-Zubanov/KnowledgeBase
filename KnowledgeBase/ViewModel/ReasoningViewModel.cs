using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.InputOutputVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.ViewModel
{
    public class ReasoningViewModel
    {
        public IList<ILinguisticVariable> InputLinguisticVariable { get; }
        public IDictionary<int, IList<ILinguisticVariable>> Reasoning { get; } = new Dictionary<int, IList<ILinguisticVariable>>();

        public ReasoningViewModel(IList<ILinguisticVariable> inputLinguistics, IDictionary<int, IList<ILinguisticVariable>> reasoning)
        {
            InputLinguisticVariable = inputLinguistics;
            foreach (var pair in reasoning)
                Reasoning.Add(pair.Key + 1, pair.Value);
        }
    }
}
