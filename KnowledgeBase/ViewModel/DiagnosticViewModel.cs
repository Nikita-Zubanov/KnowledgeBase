using KnowledgeBase.Views;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.InputOutputVariables;
using KnowledgeBaseLibrary.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnowledgeBase.ViewModel
{
    class DiagnosticViewModel
    {
        IList<string> Output { get; }
        public DiagnosticViewModel(IList<IParameter> parameters)
        {
            LogicalOutput logicalOutput = new LogicalConclusion(parameters).GetLogicalOutput();

            Dictionary<int, IList<string>> logicalOutputStr = new Dictionary<int, IList<string>>();
            foreach (KeyValuePair<int, IList<Judgment>> keyValue in logicalOutput.FactorsOutput)
            {
                logicalOutputStr[keyValue.Key] = new List<string>();
                foreach (Judgment judgment in keyValue.Value)
                {
                    logicalOutputStr[keyValue.Key].Add(judgment.ToString());
                }
            }
        }
    }
}
