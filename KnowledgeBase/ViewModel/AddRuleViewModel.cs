using KnowledgeBase.Models;
using KnowledgeBase.Views;
using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.RuleVariables;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace KnowledgeBase.ViewModel
{
    class AddRuleViewModel
    {
        private IList<ILinguisticVariable> linguistics;
        private IList<FactorFuzzyValue> termSets;

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

        public AddRuleViewModel()       
        {
            Initialize();
        }

        private void Initialize()
        {
            using (DBManager db = new DBManager(DBManager.ConnectionString))
            {
                linguistics = db.GetLinguisticVariables();
                termSets = db.GetTermSets();
            }

            AntecedentM = new AntecedentM(linguistics, termSets);
            AntecedentM.Initialize();
            ConsequentM = new ConsequentM(linguistics, termSets);
        }

        private void AddRule(object obj)
        {
            LinguisticVariable linguistic = (LinguisticVariable) linguistics.Where(lv => lv.Title == ConsequentM.SelectedTitle).First();
            Antecedent antecedent = AntecedentM.Make();
            Judgment consequent = ConsequentM.Make();

            Rule newRule = new SimpleRule(linguistic, antecedent, consequent);
            SaveToDB(newRule);
        }
        private bool CanAddRule(object obj)
        {
            return AntecedentM.IsVerified() && ConsequentM.IsVerified();
        }

        private void SaveToDB(Rule rule)
        {
            using (DBManager db = new DBManager(DBManager.ConnectionString))
            {
                db.InsertRule(rule);
            }
        }
    }
}
