using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KnowledgeBase.ViewModel
{
    public class LinguisticVariableForView : ILinguisticVariable
    {
        public FactorTitle Title { get; }
        public FactorFuzzyValue FuzzyValue { get; }
        public decimal? Value { get; }

        public Dictionary<FactorFuzzyValue, decimal[]> ValueRanges { get; }
        public IList<FactorFuzzyValue> TermSet { get; }

        public string Description { get; }
        public string Unit { get; }

        public int ValueInPercent
        {
            get
            {
                decimal min = 0;
                decimal max = 1;

                if (ValueRanges != null)
                    if (ValueRanges.ContainsKey(FactorFuzzyValue.Maximum))
                    {
                        min = ValueRanges[FactorFuzzyValue.Maximum][0];
                        max = ValueRanges[FactorFuzzyValue.Maximum][1];
                    }
                    else
                    {
                        min = ValueRanges[FactorFuzzyValue.Successful][0];
                        max = ValueRanges[FactorFuzzyValue.Successful][1];
                    }

                int ratioValueToMaximumInPercent = Convert.ToInt32(((Value - min) * 100)/ (max - min));
                return ratioValueToMaximumInPercent;
            }
            set { }
        }

        public Dictionary<int, string> Colors { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> Values { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, decimal> Offsets { get; set; } = new Dictionary<int, decimal>();
        public string Red { get; set; } = "Red";
        public LinguisticVariableForView(
            FactorTitle title,
            FactorFuzzyValue fuzzyValue,
            decimal? value,
            Dictionary<FactorFuzzyValue, decimal[]> valueRanges,
            IList<FactorFuzzyValue> termSet,
            string description,
            string unit
            )
        {
            this.Title = title;
            this.FuzzyValue = fuzzyValue;
            this.Value = value;
            this.ValueRanges = valueRanges;
            this.TermSet = termSet;
            this.Description = description;
            this.Unit = unit;

            if (valueRanges != null &&
                valueRanges.Count == 3)
            {
                if (valueRanges[FactorFuzzyValue.Nominal][0] == valueRanges[FactorFuzzyValue.Maximum][0])
                {
                    Colors[1] = "Lime";
                    Colors[2] = "Lime";
                    Colors[3] = "Yellow";
                    Colors[4] = "Orange";
                    Colors[5] = "Red";

                    Values[1] = valueRanges[FactorFuzzyValue.Nominal][0].ToString();
                    Values[2] = "";
                    Values[3] = valueRanges[FactorFuzzyValue.Nominal][1].ToString();
                    Values[4] = "";
                    Values[5] = valueRanges[FactorFuzzyValue.Maximum][1].ToString();

                    decimal min = ValueRanges[FactorFuzzyValue.Maximum][0];
                    decimal max = ValueRanges[FactorFuzzyValue.Maximum][1];
                    decimal endNominal = ValueRanges[FactorFuzzyValue.Nominal][1];
                    decimal middleRange = (endNominal - min) / (max - min);
                    Offsets[1] = 0;
                    Offsets[2] = 0;
                    Offsets[3] = middleRange;
                    Offsets[4] = 0.85m;
                    Offsets[5] = 1;

                }
                else
                {
                    Colors[1] = "Red";
                    Colors[2] = "Yellow";
                    Colors[3] = "Lime";
                    Colors[4] = "Yellow";
                    Colors[5] = "Red";

                    Values[1] = valueRanges[FactorFuzzyValue.Maximum][0].ToString();
                    Values[2] = valueRanges[FactorFuzzyValue.Nominal][0].ToString();
                    Values[3] = "";
                    Values[4] = valueRanges[FactorFuzzyValue.Nominal][1].ToString();
                    Values[5] = valueRanges[FactorFuzzyValue.Maximum][1].ToString();

                    decimal min = ValueRanges[FactorFuzzyValue.Maximum][0];
                    decimal max = ValueRanges[FactorFuzzyValue.Maximum][1];
                    decimal startNominal = ValueRanges[FactorFuzzyValue.Nominal][0];
                    decimal endNominal = ValueRanges[FactorFuzzyValue.Nominal][1];
                    decimal startMiddle = (startNominal - min) / (max - min);
                    decimal endMiddle = (endNominal - min) / (max - min);
                    Offsets[1] = 0;
                    Offsets[2] = startMiddle;
                    Offsets[3] = 0.5m;
                    Offsets[4] = endMiddle;
                    Offsets[5] = 1;
                }
            }
            else if (valueRanges != null &&
                valueRanges.Count == 2)
            {
                Colors[1] = "Lime";
                Colors[2] = "Lime";
                Colors[3] = "Yellow";
                Colors[4] = "Orange";
                Colors[5] = "Red";

                Values[1] = valueRanges[FactorFuzzyValue.Successful][0].ToString();
                Values[2] = "";
                Values[3] = "";
                Values[4] = "";
                Values[5] = valueRanges[FactorFuzzyValue.Successful][1].ToString();

                Offsets[1] = 0;
                Offsets[2] = 0.25m;
                Offsets[3] = 0.75m;
                Offsets[4] = 0.9m;
                Offsets[5] = 1;
            }
            else
            {
                Colors[1] = "Lime";
                Colors[2] = "Lime";
                Colors[3] = "Red";
                Colors[4] = "Red";
                Colors[5] = "Red";

                Values[1] = "Есть";
                Values[2] = "";
                Values[3] = "";
                Values[4] = "";
                Values[5] = "Нет";

                Offsets[1] = 0;
                Offsets[2] = 0.25m;
                Offsets[3] = 0.7m;
                Offsets[4] = 0.85m;
                Offsets[5] = 1;
            }
        }

        public static explicit operator LinguisticVariableForView(LinguisticVariable linguistic)
        {
            return new LinguisticVariableForView(
                linguistic.Title,
                linguistic.FuzzyValue,
                linguistic.Value,
                linguistic.ValueRanges, 
                linguistic.TermSet,
                linguistic.Description,
                linguistic.Unit
                );
        }
    }
    public class DataAnalyticsViewModel
    {
        public IList<LinguisticVariableForView> InputLinguisticVariables { get; set; }

        public DataAnalyticsViewModel(IList<ILinguisticVariable> linguistics)
        {
            SetLinguisticVarsForView(linguistics);
        }

        private void SetLinguisticVarsForView(IList<ILinguisticVariable> linguisticVars)
        {
            InputLinguisticVariables = new List<LinguisticVariableForView>();
            foreach (LinguisticVariable linguistic in linguisticVars)
                InputLinguisticVariables.Add((LinguisticVariableForView)linguistic);
        }
    }
}
