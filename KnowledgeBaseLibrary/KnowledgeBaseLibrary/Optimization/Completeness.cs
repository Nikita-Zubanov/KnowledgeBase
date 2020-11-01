using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal class Completeness : IVerification, ITransformer
    {
        public void Transform(ref IList<Rule> rules)
        {
            IList<Rule> treeRules = GetTopRules(rules);

            MakeTreeRules(rules, treeRules);

            rules = treeRules;
        }

        public bool IsVerified(IList<Rule> rules)
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

        private void MakeTreeRules(IList<Rule> baseRules, IList<Rule> rulesTree)
        {
            foreach (Rule ruleOfTree in rulesTree)
            {
                bool isAdded = false;

                foreach (Rule baseRule in baseRules)
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

        private IList<Rule> GetTopRules(IList<Rule> baseRules)
        {
            IList<Rule> topRules = new List<Rule>();

            foreach (Rule currentRule in baseRules)
            {
                bool isTopRule = true;

                foreach (Rule rule in baseRules)
                    if (rule.Antecedent.Contains(currentRule.Consequent))
                    {
                        isTopRule = false;
                        break;
                    }

                if (isTopRule)
                {
                    Rule topRule = new CompoundRule(currentRule.LinguisticVariable, currentRule.Antecedent, currentRule.Consequent);
                    topRules.Add(topRule);
                }
            }

            return topRules;
        }
    }
}
