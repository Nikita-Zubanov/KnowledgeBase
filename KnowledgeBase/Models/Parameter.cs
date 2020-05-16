using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase
{
    class Parameter : INotifyPropertyChanged
    {
        private string designation;
        private string title;
        private string unit;
        private string normalizedValues;
        private string methodMeasurement;
        private string[] termSet;
        private string value;

        public string Designation
        {
            get { return designation; }
            set
            {
                designation = value;
                OnPropertyChanged("Designation");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged("Unit");
            }
        }
        public string NormalizedValues
        {
            get { return normalizedValues; }
            set
            {
                normalizedValues = value;
                OnPropertyChanged("NormalizedValues");
            }
        }
        public string MethodMeasurement
        {
            get { return methodMeasurement; }
            set
            {
                methodMeasurement = value;
                OnPropertyChanged("MethodMeasurement");
            }
        }
        public string[] TermSet
        {
            get { return termSet; }
            set
            {
                termSet = value;
                OnPropertyChanged("TermSet");
            }
        }
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static IList<Parameter> GetParameters()
        {
            return new List<Parameter>
            {
                new Parameter { Title = "Длительность провала напряжения", Unit = "сек." },
                new Parameter { Title = "Коэффициент несимметрии напряжений по обратной последовательности", Unit = "%" },
                new Parameter { Title = "Коэффициент n-ой гармонической составляющей напряжения", Unit = "%" },
                new Parameter { Title = "Погодные условия" },
                new Parameter { Title = "Напряжения по фазе С, UC", Unit = "В" },
                new Parameter { Title = "Напряжения по фазе А, UA", Unit = "В" },
                new Parameter { Title = "Напряжения по фазе В, UB", Unit = "В" },
            };
        }
    }
}
