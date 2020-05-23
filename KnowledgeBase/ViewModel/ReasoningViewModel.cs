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
        public IDictionary<int, IList<ILinguisticVariable>> Reasoning { get; }

        public ReasoningViewModel(IList<ILinguisticVariable> inputLinguistics, IDictionary<int, IList<ILinguisticVariable>> reasoning)
        {
            InputLinguisticVariable = inputLinguistics;
            Reasoning = reasoning;
        }
    }
}
