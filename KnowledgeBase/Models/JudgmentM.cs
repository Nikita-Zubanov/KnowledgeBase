using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.Models
{
    public class JudgmentM : INotifyPropertyChanged
    {
        private string title;
        private string fuzzyValue;
        private string connection;
        private string initialBracket;
        private string endBracket;

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
        public string Connection
        {
            get { return connection; }
            set
            {
                connection = value;
                OnPropertyChanged("Connection");
            }
        }
        public string InitialBracket
        {
            get { return initialBracket; }
            set
            {
                initialBracket = value;
                OnPropertyChanged("InitialBracket");
            }
        }
        public string EndBracket
        {
            get { return endBracket; }
            set
            {
                endBracket = value;
                OnPropertyChanged("EndBracket");
            }
        }

        Action Add;
        public event PropertyChangedEventHandler PropertyChanged;


        public JudgmentM(string title, string fuzzyValue, string connection, Action action)
        {
            this.title = title;
            this.fuzzyValue = fuzzyValue;
            this.connection = connection;
            Add = action;
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (Add != null)
                    Add.Invoke();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
