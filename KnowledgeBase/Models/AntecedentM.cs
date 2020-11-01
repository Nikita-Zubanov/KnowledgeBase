using KnowledgeBaseLibrary;
using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace KnowledgeBase.Models
{
    class AntecedentM 
    {
        private IList<ILinguisticVariable> linguistics;
        private IList<FactorFuzzyValue> termSets;

        public ObservableCollection<JudgmentM> Judgments { get; set; } = new ObservableCollection<JudgmentM>();

        public AntecedentM(
            IList<ILinguisticVariable> linguistics,
            IList<FactorFuzzyValue> termSets,
            Rule rule) : this(linguistics, termSets)
        {
            SetJudgmentsByRule(rule);
        }
        public AntecedentM(
           IList<ILinguisticVariable> linguistics,
           IList<FactorFuzzyValue> termSets)
        {
            this.linguistics = linguistics;
            this.termSets = termSets;
        }

        public void Initialize()
        {
            Judgments.Add(new JudgmentM(linguistics, termSets, AddNewJudgmentFromAntecedent));
        }

        public Antecedent Make()
        {
            Antecedent antecedent = new Antecedent();

            IDictionary<int, IList<Judgment>> judgmentsDict = GetJudgmentDictionary();
            foreach (IList<Judgment> judgments in judgmentsDict.Values)
                if (judgments.Count == 1)
                    antecedent.AND(judgments.First());
                else
                    antecedent.OR(judgments.ToArray());

            return antecedent;
        }

        public bool IsVerified()
        {
            return IsNotNull() && IsCorrectFuzzyVariable();
        }
        private bool IsNotNull()
        {
            if (Judgments.Any(jm => jm.SelectedTitle == null || jm.SelectedFuzzyValue == null))
                return false;
            return true;
        }
        private bool IsCorrectFuzzyVariable()
        {
            foreach (JudgmentM judgment in Judgments)
            {
                FactorTitle title = (FactorTitle)judgment.SelectedTitle;
                FactorFuzzyValue fuzzyValue = (FactorFuzzyValue)judgment.SelectedFuzzyValue;

                ILinguisticVariable linguistic = linguistics.Where(lv => lv.Title == title).First();
                if (!linguistic.TermSet.Contains(fuzzyValue))
                    return false;
            }
            return true;
        }

        private void SetJudgmentsByRule(Rule rule)
        {
            for (int i = 0; i < rule.Antecedent.JudgmentDictionary.Count; i++)
            {
                IList<Judgment> judgments = rule.Antecedent.JudgmentDictionary[i];
                
                if (judgments.Count == 1)
                {
                    Judgments.Add(new JudgmentM(
                        linguistics,
                        termSets,
                        judgments.First().Title,
                        judgments.First().FuzzyValue,
                        LogicalConnection.AND));
                }
                else
                    foreach (Judgment judgment in judgments)
                    {
                        Judgments.Add(new JudgmentM(
                            linguistics,
                            termSets,
                            judgment.Title,
                            judgment.FuzzyValue,
                            LogicalConnection.OR));
                    }

                Judgments.Last().SelectedConnection = LogicalConnection.AND;
            }
        }
        private IDictionary<int, IList<Judgment>> GetJudgmentDictionary()
        {
            IDictionary<int, IList<Judgment>> judgmentsDict = new Dictionary<int, IList<Judgment>>();
            int counter = 0;
            judgmentsDict.Add(counter, new List<Judgment>());

            Judgment currentJudgment = new Judgment((FactorTitle)Judgments.First().SelectedTitle, (FactorFuzzyValue)Judgments.First().SelectedFuzzyValue);
            if (Judgments.First().SelectedConnection == LogicalConnection.AND)
            {
                judgmentsDict[counter].Add(currentJudgment);

                counter++;
                judgmentsDict.Add(counter, new List<Judgment>());
            }
            else
                judgmentsDict[counter].Add(currentJudgment);

            for (int i = 1; i < Judgments.Count; i++)
            {
                currentJudgment = new Judgment((FactorTitle)Judgments[i].SelectedTitle, (FactorFuzzyValue)Judgments[i].SelectedFuzzyValue);
                if (Judgments[i - 1].SelectedConnection == LogicalConnection.OR)
                {
                    judgmentsDict[counter].Add(currentJudgment);
                }
                else
                {
                    counter++;
                    judgmentsDict.Add(counter, new List<Judgment>());

                    judgmentsDict[counter].Add(currentJudgment);
                }
            }

            return judgmentsDict;
        }

        private void AddNewJudgmentFromAntecedent()
        {
            if (Judgments.Last().SelectedTitle != null &&
                Judgments.Last().SelectedFuzzyValue != null &&
                Judgments.Last().SelectedConnection != null)
                Judgments.Add(new JudgmentM(
                    linguistics, 
                    termSets,
                    AddNewJudgmentFromAntecedent));
        }
    }
}
