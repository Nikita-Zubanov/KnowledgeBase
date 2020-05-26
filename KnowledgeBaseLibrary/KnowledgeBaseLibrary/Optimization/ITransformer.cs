using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal interface ITransformer
    {
        void Transform(ref IList<KB.Rule> rules);
    }
}
