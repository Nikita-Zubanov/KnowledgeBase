using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.Optimization;
using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Modules
{
    internal class RuleBase
    {
        private IList<Rule> ruleList;
        private IList<Rule> ruleTree;

        public IList<Rule> RuleList { get { return ruleList; } private set { ruleList = value; } }
        public IList<Rule> RuleTree { get { return ruleTree; } private set { ruleTree = value; } }

        public RuleBase()
        {
            RuleList = new List<Rule>();
            RuleTree = new List<Rule>();

            this.Initialize();
        }

        public IList<Judgment> GetTopRuleConsequences()
        {
            IList<Judgment> topRuleConsequences = new List<Judgment>();

            foreach (Rule rule in RuleTree)
                topRuleConsequences.Add(rule.Consequent);

            return topRuleConsequences;
        }

        private void Initialize()
        {
            this.UploadData();

            this.Transform(new Coherence());
            this.Transform(new Completeness());
        }

        private void Transform(ITransformer verification)
        {
            verification.Transform(ref ruleTree);
        }

        private bool IsVerified(IVerification verification)
        {
            return verification.IsVerified(RuleList);
        }

        private void UploadData()
        {
            using (DBManager db = new DBManager(DBManager.ConnectionString))
            {
                RuleList = db.GetRules();
                RuleTree = db.GetRules();
            }
        }

    }
}