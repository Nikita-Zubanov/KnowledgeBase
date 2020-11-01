using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal class Coherence : IVerification, ITransformer
    {
        public void Transform(ref IList<Rule> rules)
        {
            this.RemoveDuplicates(rules);
        }

        public bool IsVerified(IList<Rule> rules)
        {
            if (rules != null)
            {
                if (this.IsConsequentsNotContradictions(rules))
                    return true;

                return false;
            }
            else
                throw new NullReferenceException();
        }

        private void RemoveDuplicates(IList<Rule> rules)
        {
            for (Int32 index = 0; index < rules.Count - 1; index++)
            {
                Rule currentRule = rules[index];
                Rule nextRule = rules[index + 1];

                if (currentRule.Equals(nextRule))
                    rules.Remove(nextRule);
            }
        }

        private bool IsConsequentsNotContradictions(IList<Rule> rules)
        {
            foreach (Rule currentRule in rules)
                foreach (Rule rule in rules)
                    if (currentRule.Antecedent.Equals(rule.Antecedent) &&
                        !currentRule.Consequent.Equals(rule.Consequent))
                        return false;

            return true;
        }
    }
}
