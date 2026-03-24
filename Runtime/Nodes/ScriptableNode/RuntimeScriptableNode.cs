using System;
using System.Collections.Generic;

namespace SimpleDialogues.Runtime
{
    [Serializable]
    public class RuntimeScriptableNode : RuntimeBaseNode
    {
        public ScriptableNodeEvaluator evaluator;
        public List<string> nextNodesID;
    }
}