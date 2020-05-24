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
    class ParameterM : IParameter, INotifyPropertyChanged
    {
        private FactorTitle title;
        private string description;
        private string valueString = "";
        private string unit;

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
                        valueString = $"0.";
                    else
                    {
                        int a = Convert.ToInt32(value);
                        valueString = a.ToString();
                    }
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
                new ParameterM { Title = FactorTitle.dt, Description = "Длительность провала напряжения", Unit = "сек." },
                new ParameterM { Title = FactorTitle.K2U, Description = "Коэффициент несимметрии напряжений по обратной последовательности", Unit = "%" },
                new ParameterM { Title = FactorTitle.KUn, Description = "Коэффициент n-ой гармонической составляющей напряжения", Unit = "%" },
                new ParameterM { Title = FactorTitle.Weather, Description = "Погодные условия" },
                new ParameterM { Title = FactorTitle.Ua, Description = "Напряжения по фазе А", Unit = "В" },
                new ParameterM { Title = FactorTitle.Ub, Description = "Напряжения по фазе В", Unit = "В" },
                new ParameterM { Title = FactorTitle.Uc, Description = "Напряжения по фазе С", Unit = "В" },
            };
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}) = {2}{3}", Description, title.ToString(), valueString, unit);
        }
    }
}
