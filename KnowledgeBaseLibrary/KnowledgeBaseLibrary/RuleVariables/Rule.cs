using KnowledgeBaseLibrary.FundamentalVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.RuleVariables
{
    public abstract class Rule
    {
        public LinguisticVariable LinguisticVariable { get; private set; }
        public Antecedent Antecedent { get; private set; }
        public Judgment Consequent { get; private set; }

        public Rule(LinguisticVariable linguisticVariable, Antecedent antecedent, Judgment consequent)
        {
            this.LinguisticVariable = linguisticVariable;
            this.Antecedent = antecedent;
            this.Consequent = consequent;
        }

        public abstract void AddChildRule(Rule rule);
        public abstract IList<Rule> GetChildRules();

        public bool IsAllowConsequent(IList<Judgment> judgments)
        {
            return Antecedent.IsTrue(judgments);
        }

        public override bool Equals(object obj)
        {
            if (obj is Rule)
            {
                Rule rule = obj as Rule;

                if (this.Antecedent.Equals(rule.Antecedent) &&
                    this.Consequent.Equals(rule.Consequent))
                    return true;
                else
                    return false;
            }
            else
                throw new NullReferenceException();
        }

        public override string ToString()
        {
            string consequentStr = $"Consequent: { this.Consequent.ToString() };";

            return $"{ LinguisticVariable.Title }.{ consequentStr }";
        }
    }
}
