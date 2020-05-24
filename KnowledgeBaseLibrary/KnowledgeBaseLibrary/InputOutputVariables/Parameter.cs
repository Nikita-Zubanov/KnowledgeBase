using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.InputOutputVariables
{
    internal class Parameter : IParameter
    {
        public FactorTitle Title { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public string Unit { get; set; }

        public Parameter(FactorTitle title, string description, int value, string unit)
        {
            this.Title = title;
            this.Description = description;
            this.Value = value;
            this.Unit = unit;
        }
    }
}
