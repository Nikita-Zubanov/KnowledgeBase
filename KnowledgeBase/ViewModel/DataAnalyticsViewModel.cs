using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.ViewModel
{
    public class DataAnalyticsViewModel
    {
        public IList<LinguisticVariable> InputLinguisticVariable { get; }

        public DataAnalyticsViewModel(IList<LinguisticVariable> linguisticVars)
        {
            InputLinguisticVariable = linguisticVars;
        }
    }
}
