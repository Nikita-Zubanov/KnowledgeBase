using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.FundamentalVariables
{
    public class Judgment
    {
        public FactorTitle Title { get; private set; }
        public FactorFuzzyValue FuzzyValue { get; private set; }

        public Judgment(FactorTitle title, FactorFuzzyValue fuzzyValue)
        {
            Title = title;
            FuzzyValue = fuzzyValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Judgment)
            {
                Judgment judgment = obj as Judgment;

                if (Title == judgment.Title &&
                    FuzzyValue == judgment.FuzzyValue)
                    return true;
                else
                    return false;
            }
            else
                throw new ArgumentException();
        }

        public override string ToString()
        {
            return String.Format("{0} — {1}", Title.ToString(), FuzzyValue.ToString());
        }
    }
}
