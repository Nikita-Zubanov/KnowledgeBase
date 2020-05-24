using KnowledgeBaseLibrary.FundamentalVariables;
using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowledgeBaseLibrary.InputOutputVariables
{
    public class LogicalOutput
    {
        private IList<Judgment> allFactors = new List<Judgment>();
        private IList<ILinguisticVariable> allLinguisticVariables = new List<ILinguisticVariable>();
        private Int32 iteration = 0;

        internal IDictionary<int, IList<Judgment>> FactorsOutput { get; private set; } = new Dictionary<int, IList<Judgment>>();
        internal IDictionary<int, IList<ILinguisticVariable>> LinguisticVariablesOutput { get; private set; } = new Dictionary<int, IList<ILinguisticVariable>>();
        internal IList<Judgment> CurrentFactors { get { return FactorsOutput[iteration]; } }

        public IList<ILinguisticVariable> InputLinguisticVariable
        { 
            get 
            {
                IList<ILinguisticVariable> linguisticVars = LinguisticVariablesOutput.First().Value.ToList();
                return linguisticVars;
            } 
        }
        public IDictionary<int, IList<ILinguisticVariable>> Reasoning
        { 
            get
            {
                IDictionary<int, IList<ILinguisticVariable>> reasoning = new Dictionary<int, IList<ILinguisticVariable>>();
                int inputFactorsIndex = 1;
                int conclusionIndex = LinguisticVariablesOutput.Count - 1;
                int additionalLinguisticVarsIndex = LinguisticVariablesOutput.Count;
                int counter = 0;

                foreach (var keyValue in LinguisticVariablesOutput)
                    if (keyValue.Key != inputFactorsIndex && keyValue.Key != conclusionIndex && keyValue.Key != additionalLinguisticVarsIndex)
                    reasoning.Add(counter++, keyValue.Value.ToList());
                return reasoning;
            } 
        }
        public ILinguisticVariable Conclusion 
        {
            get
            {
                int conclusionIndex = LinguisticVariablesOutput.Count - 1;

                foreach (var keyValue in LinguisticVariablesOutput)
                    if (keyValue.Key == conclusionIndex)
                        return keyValue.Value.First();

                throw new NullReferenceException("Отсутствует логический вывод. Либо нарушена \"композиция\" FactorsOutput, которая заключается в: " +
                    "0-ой элемент - входные факторы, с 1-ого по n-ый элементы - логическое рассуждение, предпоследний элемент - логический вывод, последний - дополнительные суждения");
            }
        }
        public IList<ILinguisticVariable> AdditionalLinguisticVariables
        { 
            get
            {
                return LinguisticVariablesOutput.Last().Value.ToList(); 
            } 
        }

        public LogicalOutput()
        {
            using (DBManager db = new DBManager())
            {
                allLinguisticVariables = db.GetLinguisticVariables();
            }
        }

        public void Next()
        {
            iteration++;

            FactorsOutput[iteration] = new List<Judgment>();
            LinguisticVariablesOutput[iteration] = new List<ILinguisticVariable>();
        }

        public void Add(Judgment judgment)
        {
            FactorsOutput[iteration].Add(judgment);
            allFactors.Add(judgment);

            ILinguisticVariable linguisticVar = allLinguisticVariables.Where(lv => lv.Title == judgment.Title).FirstOrDefault();
            linguisticVar = new LinguisticVariable(
                linguisticVar.Title,
                judgment.FuzzyValue, 
                linguisticVar.ValueRanges, 
                linguisticVar.TermSet, 
                linguisticVar.Description, 
                linguisticVar.Unit);
            LinguisticVariablesOutput[iteration].Add(linguisticVar);
        }
        public void Add(ILinguisticVariable linguisticVar)
        {
            Judgment judgment = new Judgment(linguisticVar.Title, linguisticVar.FuzzyValue);
            FactorsOutput[iteration].Add(judgment);
            allFactors.Add(judgment);

            LinguisticVariablesOutput[iteration].Add(linguisticVar);
        }

        public bool HaveConsequent(KB.Rule rule)
        {
            return allFactors.Contains(rule.Consequent);
        }
    }
}
