using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal class Coherence : IVerification, ITransformer
    {
        public void Transform(ref IList<KB.Rule> rules)
        {
            this.RemoveDuplicates(rules);
        }

        public bool IsVerified(IList<KB.Rule> rules)
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

        private void RemoveDuplicates(IList<KB.Rule> rules)
        {
            for (Int32 index = 0; index < rules.Count - 1; index++)
            {
                KB.Rule currentRule = rules[index];
                KB.Rule nextRule = rules[index + 1];

                if (currentRule.Equals(nextRule))
                    rules.Remove(nextRule);
            }
        }

        private bool IsConsequentsNotContradictions(IList<KB.Rule> rules)
        {
            foreach (KB.Rule currentRule in rules)
                foreach (KB.Rule rule in rules)
                    if (currentRule.Antecedent.Equals(rule.Antecedent) &&
                        !currentRule.Consequent.Equals(rule.Consequent))
                        return false;

            return true;
        }
    }
}
