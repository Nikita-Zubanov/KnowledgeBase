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
    class AntecedentM : Antecedent, INotifyPropertyChanged
    {
        public ObservableCollection<JudgmentM> Judgments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AntecedentM()
        {
            Judgments = new ObservableCollection<JudgmentM>();
            Judgments.Add(new JudgmentM("", "", "И", Add));

        }

        public void Add()
        {
            if (Judgments.Last().FuzzyValue != "")
                Judgments.Add(new JudgmentM("", "", "И", Add));
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
