using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.InputOutputVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowledgeBaseLibrary.FundamentalVariables
{
    public class LinguisticVariable : ILinguisticVariable
    {
        public FactorTitle Title { get; private set; }
        public FactorFuzzyValue FuzzyValue { get; private set; }
        public decimal? Value { get; private set; }

        public Dictionary<FactorFuzzyValue, decimal[]> ValueRanges { get; private set; }
        public IList<FactorFuzzyValue> TermSet { get; private set; }

        public string Description { get; private set; }
        public string Unit { get; private set; }

        public LinguisticVariable(
            FactorTitle title,
            decimal? value,
            Dictionary<FactorFuzzyValue, decimal[]> valueRanges,
            IList<FactorFuzzyValue> termSet,
            string description,
            string unit
            ) : this(title, valueRanges, termSet, description, unit)
        {
            this.Value = value;
            this.FuzzyValue = GetFuzzyValueFromTermSet(value);
        }
        public LinguisticVariable(
            FactorTitle title,
            FactorFuzzyValue fuzzyValue,
            Dictionary<FactorFuzzyValue, decimal[]> valueRanges,
            IList<FactorFuzzyValue> termSet,
            string description,
            string unit
            ) : this(title, valueRanges, termSet, description, unit)
        {
            this.FuzzyValue = fuzzyValue;
        }
        public LinguisticVariable(
            FactorTitle title,
            Dictionary<FactorFuzzyValue, decimal[]> valueRanges,
            IList<FactorFuzzyValue> termSet,
            string description,
            string unit
            )
        {
            this.Title = title;
            this.ValueRanges = valueRanges;
            this.TermSet = termSet;
            this.Description = description;
            this.Unit = unit;
        }

        public override string ToString()
        {
            return Title.ToString();
        }

        private FactorFuzzyValue GetFuzzyValueFromTermSet(decimal? value)
        {
            if (ValueRanges == null)
            {
                this.Value = 0.0m;
                return GetBooleanFuzzyValue(value);
            }
            else
                return GetFuzzyValue(value);
        }
        private FactorFuzzyValue GetBooleanFuzzyValue(decimal? value)
        {
            switch (value)
            {
                case 0:
                    return FactorFuzzyValue.Unsuccessful;
                case 1:
                    return FactorFuzzyValue.Successful;
                default:
                    throw new ArgumentException("Значение параметра, принимающего лишь 0 или 1, не соответствует таковым.");
            }
        }
        private FactorFuzzyValue GetFuzzyValue(decimal? value)
        {
            foreach (KeyValuePair<FactorFuzzyValue, decimal[]> valueRange in ValueRanges)
            {
                decimal? min = valueRange.Value.First();
                decimal? max = valueRange.Value.Last();

                if (value >= min && value <= max)
                    return valueRange.Key;
            }
            throw new ArgumentException("Значение параметра не соответствует нечётким переменным в терм-множестве.");
        }
    }
}
