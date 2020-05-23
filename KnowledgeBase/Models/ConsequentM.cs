using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.Models
{
    class ConsequentM : INotifyPropertyChanged
    {
        private string title;
        private string fuzzyValue;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string FuzzyValue
        {
            get { return fuzzyValue; }
            set
            {
                fuzzyValue = value;
                OnPropertyChanged("FuzzyValue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
