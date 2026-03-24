using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogues.Runtime
{
    public class RuntimeDialogueTree : ScriptableObject
    {
        public string startingNodeID;
        [SerializeReference]
        // public Dictionary<string, RuntimeBaseNode> nodes = new Dictionary<string, RuntimeBaseNode>();
        public RuntimeNodeLUT lookUpTable = new RuntimeNodeLUT();
        
        public int nodeCount;
    }
}