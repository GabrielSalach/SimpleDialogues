using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogues.Runtime
{
    [Serializable]
    internal class RuntimeNodeLUTElement
    {
        public string nodeID;
        [SerializeReference]
        public RuntimeBaseNode runtimeNode;
    }
    
    [Serializable]
    public class RuntimeNodeLUT
    {
        [SerializeField]
        private List<RuntimeNodeLUTElement> elements = new List<RuntimeNodeLUTElement>();

        public void AddNode(string nodeID, RuntimeBaseNode runtimeNode)
        {
            elements.Add(new RuntimeNodeLUTElement()
            {
                nodeID = nodeID,
                runtimeNode = runtimeNode
            });
        }

        public Dictionary<string, RuntimeBaseNode> GetLookUpTable()
        {
            Dictionary<string, RuntimeBaseNode> dictionary = new Dictionary<string, RuntimeBaseNode>();

            foreach (RuntimeNodeLUTElement element in elements)
            {
                dictionary.Add(element.nodeID, element.runtimeNode);
            }
            
            return dictionary;
        }
    }
}