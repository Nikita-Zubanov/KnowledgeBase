using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.InputOutputVariables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase
{
    class Parameter : IParameter, INotifyPropertyChanged
    {
        private FactorTitle title { get; set; }
        private string description { get; set; }
        private string valueString { get; set; } = "";
        private string unit { get; set; }

        public FactorTitle Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public string ValueString
        {
            set
            {
                if (!valueString.Contains(",") && !valueString.Contains(".") ||
                    value != "," && value != ".")
                {
                    if (value == "," || value == ".")
                        valueString = $"0{value}";
                    else
                        valueString = value;
                    OnPropertyChanged("ValueString");
                }
            }
        }
        public decimal? Value
        {
            get 
            {
                if (String.IsNullOrEmpty(valueString))
                    return null;

                valueString = valueString.Replace(',', '.');
                return Decimal.Parse(valueString, new CultureInfo("en-GB")); 
            }
        }
        public string Unit
        {
            get { return unit; }
            set
            {
                this.unit = value;
                OnPropertyChanged("Unit");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static IList<IParameter> GetParameters()
        {
            return new List<IParameter>
            {
                new Parameter { Title = FactorTitle.dt, Description = "Длительность провала напряжения", Unit = "сек." },
                new Parameter { Title = FactorTitle.K2U, Description = "Коэффициент несимметрии напряжений по обратной последовательности", Unit = "%" },
                new Parameter { Title = FactorTitle.KUn, Description = "Коэффициент n-ой гармонической составляющей напряжения", Unit = "%" },
                new Parameter { Title = FactorTitle.Weather, Description = "Погодные условия" },
                new Parameter { Title = FactorTitle.Ua, Description = "Напряжения по фазе А", Unit = "В" },
                new Parameter { Title = FactorTitle.Ub, Description = "Напряжения по фазе В", Unit = "В" },
                new Parameter { Title = FactorTitle.Uc, Description = "Напряжения по фазе С", Unit = "В" },
            };
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}) = {2}{3}", Description, title.ToString(), valueString, unit);
        }
    }
}
