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
    public class LogicalOutputViewModel
    {
        public IList<ILinguisticVariable> InputLinguisticVariable { get; }
        public IDictionary<int, IList<ILinguisticVariable>> Reasoning { get; }

        public ILinguisticVariable Conclusion { get; }
        public IList<ILinguisticVariable> AdditionalJudgments { get; }

        private Command showReasoningCommand;
        private Command showDataAnalyticsCommand;
        public ICommand ShowReasoningCommand
        {
            get
            {
                if (showReasoningCommand == null)
                {
                    showReasoningCommand = new Command(ShowReasoning);
                }
                return showReasoningCommand;
            }
        }
        public ICommand ShowDataAnalyticsCommand
        {
            get
            {
                if (showDataAnalyticsCommand == null)
                {
                    showDataAnalyticsCommand = new Command(ShowDataAnalytics);
                }
                return showDataAnalyticsCommand;
            }
        }

        public LogicalOutputViewModel(IList<IParameter> parameters)
        {
            LogicalOutput logicalOutput = new LogicalConclusion(parameters).GetLogicalOutput();

            InputLinguisticVariable = logicalOutput.InputLinguisticVariable;
            Reasoning = logicalOutput.Reasoning;
            Conclusion = logicalOutput.Conclusion;
            AdditionalJudgments = logicalOutput.AdditionalLinguisticVariables;
        }

        private void ShowReasoning(object obj)
        {
            ReasoningViewModel reasoningVM = new ReasoningViewModel(InputLinguisticVariable, Reasoning);
            ReasoningView reasoningV = new ReasoningView(reasoningVM);
            reasoningV.Show();
        }
        private void ShowDataAnalytics(object obj)
        {
            DataAnalyticsViewModel dataAnalyticsVM = new DataAnalyticsViewModel(InputLinguisticVariable);
            DataAnalyticsView dataAnalyticsV = new DataAnalyticsView(dataAnalyticsVM);
            dataAnalyticsV.Show();
        }
    }
}
