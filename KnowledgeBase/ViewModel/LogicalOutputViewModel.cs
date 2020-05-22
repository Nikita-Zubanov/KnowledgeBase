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
        public IDictionary<int, IList<Judgment>> LogicalOutput { get; set; }

        private Command diagnoseCommand;
        public ICommand DiagnoseCommand
        {
            get
            {
                if (diagnoseCommand == null)
                {
                    diagnoseCommand = new Command(this.Command);
                }
                return diagnoseCommand;
            }
        }

        public LogicalOutputViewModel(IList<IParameter> parameters)
        {
            LogicalOutput logicalOutput = new LogicalConclusion(parameters).GetLogicalOutput();
            LogicalOutput = logicalOutput.FactorsOutput;
        }

        private void Command(object obj)
        {

        }
    }
}
