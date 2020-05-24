using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.Models
{
    public class JudgmentM : INotifyPropertyChanged
    {
        private Action AddNewJudgmentFromAntecedent;

        private FactorTitle? selectedTitle;
        private FactorFuzzyValue? selectedFuzzyValue;
        private LogicalConnection? selectedConnection;

        public FactorTitle? SelectedTitle
        {
            get { return selectedTitle; }
            set { selectedTitle = value; OnPropertyChanged("SelectedTitle"); }
        }
        public FactorFuzzyValue? SelectedFuzzyValue
        {
            get { return selectedFuzzyValue; }
            set { selectedFuzzyValue = value; OnPropertyChanged("SelectedFuzzyValue"); }
        }
        public LogicalConnection? SelectedConnection
        {
            get { return selectedConnection; }
            set { selectedConnection = value; OnPropertyChanged("SelectedConnection"); }
        }

        public ObservableCollection<FactorTitle> Titles { get; set; } = new ObservableCollection<FactorTitle>();
        public ObservableCollection<FactorFuzzyValue> FuzzyValues { get; set; } = new ObservableCollection<FactorFuzzyValue>();
        public ObservableCollection<LogicalConnection> Connections { get; set; } = new ObservableCollection<LogicalConnection>();

        public event PropertyChangedEventHandler PropertyChanged;

        public JudgmentM(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets,
            Action action)
        {
            SetObservableCollections(linguistics, termSets);
            AddNewJudgmentFromAntecedent = action;
        }

        public JudgmentM(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets,
            FactorTitle title,
            FactorFuzzyValue fuzzyValue,
            LogicalConnection connection
            ) : this(linguistics, termSets, null)
        {
            SelectedTitle = title;
            SelectedFuzzyValue = fuzzyValue;
            SelectedConnection = connection;
        }

        private void SetObservableCollections(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets)
        {
            foreach (ILinguisticVariable linguistic in linguistics)
                Titles.Add(linguistic.Title);

            foreach (FactorFuzzyValue term in termSets)
                FuzzyValues.Add(term);

            Connections.Add(LogicalConnection.AND);
            Connections.Add(LogicalConnection.OR);
        }

        public void OnPropertyChanged(string propertyName)
        {
            AddNewJudgmentFromAntecedent?.Invoke();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
