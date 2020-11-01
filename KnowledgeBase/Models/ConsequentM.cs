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
    class ConsequentM : INotifyPropertyChanged
    {
        private IList<ILinguisticVariable> linguistics;
        private IList<FactorFuzzyValue> termSets;

        private FactorTitle? selectedTitle;
        private FactorFuzzyValue? selectedFuzzyValue;

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

        public ObservableCollection<FactorTitle> Titles { get; set; } = new ObservableCollection<FactorTitle>();
        public ObservableCollection<FactorFuzzyValue> FuzzyValues { get; set; } = new ObservableCollection<FactorFuzzyValue>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ConsequentM(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets)
        {
            this.linguistics = linguistics;
            this.termSets = termSets;

            SetObservableCollections();
        }
        public ConsequentM(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets,
            FactorTitle title,
            FactorFuzzyValue fuzzyValue) :this(linguistics, termSets)
        {
            SelectedTitle = title;
            SelectedFuzzyValue = fuzzyValue;
        }

        public Judgment Make()
        {
            return new Judgment((FactorTitle)selectedTitle, (FactorFuzzyValue)selectedFuzzyValue);
        }

        public bool IsVerified()
        {
            return IsNotNull() && IsCorrectFuzzyVariable();
        }
        private bool IsNotNull()
        {
            if (SelectedTitle == null || SelectedFuzzyValue == null)
                return false;
            return true;
        }
        private bool IsCorrectFuzzyVariable()
        {
            FactorTitle title = (FactorTitle)SelectedTitle;
            FactorFuzzyValue fuzzyValue = (FactorFuzzyValue)SelectedFuzzyValue;

            ILinguisticVariable linguistic = linguistics.Where(lv => lv.Title == title).First();
            if (!linguistic.TermSet.Contains(fuzzyValue))
                return false;

            return true;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetObservableCollections()
        {
            foreach (ILinguisticVariable linguistic in linguistics)
                Titles.Add(linguistic.Title);

            foreach (FactorFuzzyValue term in termSets)
                FuzzyValues.Add(term);
        }
    }
}
