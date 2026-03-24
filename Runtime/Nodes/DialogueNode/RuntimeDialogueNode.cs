using System;

namespace SimpleDialogues.Runtime
{
    [Serializable]
    public class RuntimeDialogueNode : RuntimeBaseNode
    {
        public string dialogueLine;
        public string nextNodeID;
        public bool requirePlayerInput;
    }
}