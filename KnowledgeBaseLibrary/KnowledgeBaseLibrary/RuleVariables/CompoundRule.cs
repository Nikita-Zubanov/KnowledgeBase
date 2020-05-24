using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.RuleVariables
{
    public class CompoundRule : Rule
    {
        public IList<Rule> childRules = new List<Rule>();

        public CompoundRule(LinguisticVariable linguisticVariable, Antecedent antecedent, Judgment consequent) : base(linguisticVariable, antecedent, consequent) { }

        public override void AddChildRule(Rule rule)
        {
            this.childRules.Add(rule);
        }

        public override IList<Rule> GetChildRules()
        {
            return this.childRules;
        }
    }
}
