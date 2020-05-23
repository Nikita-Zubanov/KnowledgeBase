using KnowledgeBase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnowledgeBase.ViewModel
{
    class AddRuleViewModel: INotifyPropertyChanged
    {
        public AntecedentM AntecedentM { get; set; }
        public ConsequentM ConsequentM { get; set; }

        private Command addRuleCommand;
        public ICommand AddRuleCommand
        {
            get
            {
                if (addRuleCommand == null)
                {
                    addRuleCommand = new Command(
                        param => this.AddRule(AntecedentM),
                        param => this.CanAddRule(AntecedentM));
                }
                return addRuleCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddRuleViewModel()
        {
            AntecedentM = new AntecedentM();
            ConsequentM = new ConsequentM();
        }

        private void AddRule(object obj)
        {

        }
        private bool CanAddRule(object obj)
        {
            return true;
        }
    }
}
