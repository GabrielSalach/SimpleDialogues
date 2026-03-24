using System;
using System.Collections.Generic;

namespace SimpleDialogues.Runtime
{
    [Serializable]
    public class RuntimeChoiceNode : RuntimeBaseNode
    {
        public List<string> choicesText;
        public List<string> nextNodesID;
    }
}