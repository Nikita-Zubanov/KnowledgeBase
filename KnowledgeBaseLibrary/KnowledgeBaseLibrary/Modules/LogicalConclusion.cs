using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.InputOutputVariables;
using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowledgeBaseLibrary.Modules
{
    public class LogicalConclusion
    {
        private RuleBase ruleBase;
        private WorkingMemory workingMemory;
        private LogicalOutput logicalOutput;

        public LogicalConclusion(IList<IParameter> parameters)
        {
            Initialize(parameters);
        }

        private void Initialize(IList<IParameter> parameters)
        {
            IList<ILinguisticVariable> linguisticVarsByParams = GetLinguisticVarsBy(parameters);
            IList<Judgment> judgmentsByLinguisticVars = GetJudgmentsBy(linguisticVarsByParams);

            workingMemory = new WorkingMemory(judgmentsByLinguisticVars);
            ruleBase = new RuleBase();
            logicalOutput = new LogicalOutput();

            InitializeLogicalOutput(linguisticVarsByParams);
        }
        private void InitializeLogicalOutput(IList<ILinguisticVariable> linguistics)
        {
            logicalOutput.Next();
            foreach (ILinguisticVariable lv in linguistics)
                logicalOutput.Add(lv);
        }
        
        public LogicalOutput GetLogicalOutput()
        {
            IList<Judgment> topRuleConsequences = ruleBase.GetTopRuleConsequences();

            while (!workingMemory.ContainsAny(topRuleConsequences))
            {
                logicalOutput.Next();

                FillRulesAndFactorsLogicalOutput(workingMemory, ruleBase.RuleTree);

                workingMemory.AddRangeFactors(logicalOutput.CurrentFactors);
            }

            return logicalOutput;
        }
        private void FillRulesAndFactorsLogicalOutput(WorkingMemory workingMemory, IList<KB.Rule> ruleTree)
        {
            foreach (KB.Rule rule in ruleTree)
            {
                if (!logicalOutput.HaveConsequent(rule) &&
                    rule.IsAllowConsequent(workingMemory.Factors))
                {
                    if (!IfIsTopRuleAddToWorkingMemory(rule))
                        logicalOutput.Add(rule.Consequent);

                }

                FillRulesAndFactorsLogicalOutput(workingMemory, rule.GetChildRules());
            }
        }
        private bool IfIsTopRuleAddToWorkingMemory(KB.Rule rule)
        {
            IList<Judgment> topRuleConsequences = ruleBase.GetTopRuleConsequences();

            if (topRuleConsequences.Contains(rule.Consequent))
            {
                workingMemory.AddRangeFactors(logicalOutput.CurrentFactors);
                workingMemory.AddFactor(rule.Consequent);

                if (logicalOutput.CurrentFactors.Count != 0) 
                    logicalOutput.Next();

                logicalOutput.Add(rule.Consequent);

                logicalOutput.Next();

                return true;
            }

            return false;
        }

        private IList<ILinguisticVariable> GetLinguisticVarsBy(IList<IParameter> parameters)
        {
            IList<ILinguisticVariable> linguisticVars = new List<ILinguisticVariable>();
            using (DBManager db = new DBManager())
                linguisticVars = db.GetLinguisticVariables(parameters);

            IList<ILinguisticVariable> linguisticVarsByParams = new List<ILinguisticVariable>();
            foreach (IParameter param in parameters)
                linguisticVarsByParams.Add(linguisticVars.
                    Where(lv => lv.Title == param.Title)
                    .Select(lv => new LinguisticVariable(
                        lv.Title,
                        param.Value,
                        lv.ValueRanges,
                        lv.TermSet,
                        lv.Description,
                        lv.Unit))
                    .First());

            return linguisticVarsByParams;
        }
        private IList<Judgment> GetJudgmentsBy(IList<ILinguisticVariable> linguisticVars)
        {
            IList<Judgment> judgments = new List<Judgment>();
            foreach (ILinguisticVariable var in linguisticVars)
                judgments.Add(new Judgment(var.Title, var.FuzzyValue));

            return judgments;
        }
    }
}