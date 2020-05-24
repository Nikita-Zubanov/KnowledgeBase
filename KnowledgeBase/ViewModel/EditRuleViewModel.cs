using KnowledgeBase.Models;
using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnowledgeBase.ViewModel
{
    class EditRuleViewModel : INotifyPropertyChanged
    {
        private IList<ILinguisticVariable> linguistics;
        private IList<FactorFuzzyValue> termSets;
        private Rule selectedRule;
        private Rule editedRule;
        private AntecedentM antecedentM;
        private ConsequentM consequentM;

        public ObservableCollection<Rule> Rules { get; set; } = new ObservableCollection<Rule>();
        public Rule SelectedRule
        {
            get { return selectedRule; }
            set { selectedRule = value; selectedRule = value; OnCollectionChanged("SelectedRule"); }
        }
        public AntecedentM AntecedentM
        {
            get { return antecedentM; }
            set { antecedentM = value; OnPropertyChanged("AntecedentM"); }
        }
        public ConsequentM ConsequentM
        {
            get { return consequentM; }
            set { consequentM = value; OnPropertyChanged("ConsequentM"); }
        }


        private Command updateRuleCommand;
        private Command deleteRuleCommand;
        public ICommand UpdateRuleCommand
        {
            get
            {
                if (updateRuleCommand == null)
                {
                    updateRuleCommand = new Command(
                        param => this.UpdateRule(SelectedRule),
                        param => this.CanUpdateRule(SelectedRule));
                }
                return updateRuleCommand;
            }
        }
        public ICommand DeleteRuleCommand
        {
            get
            {
                if (deleteRuleCommand == null)
                {
                    deleteRuleCommand = new Command(
                        param => this.DeleteRule(SelectedRule),
                        param => this.CanDeleteRule(SelectedRule));
                }
                return deleteRuleCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler CollectionChanged;

        public EditRuleViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            using (DBManager db = new DBManager(DBManager.ConnectionString))
            {
                foreach (Rule rule in db.GetRules())
                    Rules.Add(rule);
                linguistics = db.GetLinguisticVariables();
                termSets = db.GetTermSets();
            }
        }

        public void OnCollectionChanged(string collectionName)
        {
            SetAntecedentAndConsequent();

            CollectionChanged?.Invoke(this, new PropertyChangedEventArgs(collectionName));
        }
        public void OnPropertyChanged(string propertyName)
        {
            SetEditedRule();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateRule(object obj)
        {
            SetEditedRule();
            using(DBManager db = new DBManager(DBManager.ConnectionString))
            {
                db.UpdateRule(SelectedRule, editedRule);
            }
        }
        private bool CanUpdateRule(object obj)
        {
            return SelectedRule != null && editedRule != null;
        }
        private void DeleteRule(object obj)
        {
            SetEditedRule();
            using (DBManager db = new DBManager(DBManager.ConnectionString))
            {
                db.DeleteRule(SelectedRule);
            }
        }
        private bool CanDeleteRule(object obj)
        {
            return SelectedRule != null && editedRule != null;
        }

        private void SetAntecedentAndConsequent()
        {
            AntecedentM = new AntecedentM(
                linguistics,
                termSets,
                SelectedRule);
            ConsequentM = new ConsequentM(
                linguistics,
                termSets,
                SelectedRule.Consequent.Title,
                SelectedRule.Consequent.FuzzyValue);
        }
        private void SetEditedRule()
        {
            if (AntecedentM != null && ConsequentM != null)
            {
                LinguisticVariable linguistic = SelectedRule.LinguisticVariable;
                Antecedent antecedent = AntecedentM.Make();
                Judgment consequent = ConsequentM.Make();

                editedRule = new SimpleRule(linguistic, antecedent, consequent);
            }
        }
    }
}
