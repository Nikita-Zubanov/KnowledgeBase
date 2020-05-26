using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.FundamentalVariables
{
#pragma warning disable CS0659 // '"Judgment" переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode().
    public class Judgment
#pragma warning restore CS0659 // '"Judgment" переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode().
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
