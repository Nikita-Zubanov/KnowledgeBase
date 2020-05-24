using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal class Completeness : IVerification, ITransformer
    {
        public void Transform(ref IList<KB.Rule> rules)
        {
            IList<KB.Rule> treeRules = GetTopRules(rules);

            MakeTreeRules(rules, treeRules);

            rules = treeRules;
        }

        public bool IsVerified(IList<KB.Rule> rules)
        {
            if (rules != null)
            {
                if (this.GetTopRules(rules).Count == 0)
                    return false;

                return true;
            }
            else
                throw new NullReferenceException();
        }

        private void MakeTreeRules(IList<KB.Rule> baseRules, IList<KB.Rule> rulesTree)
        {
            foreach (KB.Rule ruleOfTree in rulesTree)
            {
                bool isAdded = false;

                foreach (KB.Rule baseRule in baseRules)
                {
                    if (ruleOfTree.Antecedent.Contains(baseRule.Consequent))
                    {
                        ruleOfTree.AddChildRule((CompoundRule)(SimpleRule)baseRule);

                        isAdded = true;
                    }
                }

                if (isAdded)
                    this.MakeTreeRules(baseRules, ruleOfTree.GetChildRules());
            }
        }

        private IList<KB.Rule> GetTopRules(IList<KB.Rule> baseRules)
        {
            IList<KB.Rule> topRules = new List<KB.Rule>();

            foreach (KB.Rule currentRule in baseRules)
            {
                bool isTopRule = true;

                foreach (KB.Rule rule in baseRules)
                    if (rule.Antecedent.Contains(currentRule.Consequent))
                    {
                        isTopRule = false;
                        break;
                    }

                if (isTopRule)
                {
                    KB.Rule topRule = new CompoundRule(currentRule.LinguisticVariable, currentRule.Antecedent, currentRule.Consequent);
                    topRules.Add(topRule);
                }
            }

            return topRules;
        }
    }
}
