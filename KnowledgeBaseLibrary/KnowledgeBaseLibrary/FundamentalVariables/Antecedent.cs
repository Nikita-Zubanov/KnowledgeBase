using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnowledgeBaseLibrary.FundamentalVariables
{
    public enum LogicalConnection
    {
        AND,
        OR
    }


    public class Antecedent
    {
        class JudgmentLine
        {
            public LogicalConnection Connection { get; private set; }
            public IList<Judgment> Judgments { get; private set; }

            public JudgmentLine(LogicalConnection connection, IList<Judgment> judgments)
            {
                this.Connection = connection;
                this.Judgments = judgments;
            }

            public bool IsTrue(IList<Judgment> judgments)
            {
                if (Connection == LogicalConnection.AND)
                    return judgments.Contains(this.Judgments.First());
                else if (Connection == LogicalConnection.OR)
                    return this.Judgments.Any(j => judgments.Contains(j));

                return false;
            }
            public bool IsCorrespondsButNotTrue(IList<Judgment> judgments)
            {
                if (Connection == LogicalConnection.AND)
                    return judgments.Any(j => j.Title == this.Judgments.First().Title);
                else if (Connection == LogicalConnection.OR)
                    return this.Judgments.Any(tj => judgments.Any(j => j.Title == tj.Title));

                return false;
            }

            public bool Contains(Judgment judgment)
            {
                return this.Judgments.Contains(judgment);
            }

            public override string ToString()
            {
                string line = "";

                foreach (Judgment judgment in Judgments)
                    line += String.Format("{0} {1} ", judgment, Connection);
                line = line.TrimEnd(new char[] { 'O', 'R', 'A', 'N', 'D', ' ' });

                return line;
            }
        }

        private IList<JudgmentLine> antecedent = new List<JudgmentLine>();

        public IDictionary<int, IList<Judgment>> JudgmentDictionary 
        {
            get
            {
                IDictionary<int , IList<Judgment>> pairs = new Dictionary<int, IList<Judgment>>();
                int index = 0;

                foreach (JudgmentLine line in antecedent)
                {
                    if (line.Connection == LogicalConnection.AND)
                        pairs[index] = new List<Judgment> { line.Judgments.First() };
                    else
                        pairs[index] = line.Judgments;

                    index++;
                }

                return pairs;
            }
        }

        public Antecedent AND(Judgment judgment)
        {
            JudgmentLine line = new JudgmentLine(LogicalConnection.AND, new List<Judgment> { judgment });

            antecedent.Add(line);

            return this;
        }
        public Antecedent OR(params Judgment[] judgments)
        {
            JudgmentLine line = new JudgmentLine(LogicalConnection.OR, judgments);

            antecedent.Add(line);

            return this;
        }

        public bool IsTrue(IList<Judgment> judgments)
        {
            Boolean isAllow = false;

            foreach (JudgmentLine judgmentLine in antecedent)
            {
                isAllow = judgmentLine.IsTrue(judgments);

                if (isAllow == false)
                    break;
            }

            return isAllow;
        }
        public bool IsCorrespondsButNotTrue(IList<Judgment> judgments)
        {
            Boolean isAllow = false;

            foreach (JudgmentLine judgmentLine in antecedent)
            {
                isAllow = judgmentLine.IsCorrespondsButNotTrue(judgments);

                if (isAllow == false)
                    break;
            }

            return isAllow;
        }

        public bool Contains(Judgment judgment)
        {
            return antecedent.Any(le => le.Contains(judgment));
        }

        public override string ToString()
        {
            string line = "";

            foreach (JudgmentLine judgmentLine in antecedent)
                line += String.Format("<{0}> AND ", judgmentLine.ToString());
            line = line.TrimEnd(new char[] { 'A', 'N', 'D', ' ' });

            return line;
        }
    }
}
