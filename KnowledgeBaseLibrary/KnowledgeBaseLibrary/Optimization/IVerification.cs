using KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeBaseLibrary.Optimization
{
    internal interface IVerification
    {
        bool IsVerified(IList<Rule> rules);
    }
}
