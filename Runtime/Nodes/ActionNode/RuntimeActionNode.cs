using System;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialogues.Runtime
{
    [Serializable]
    public class RuntimeActionNode : RuntimeBaseNode
    {
        [SerializeField]
        public UnityEvent evt;
        public string nextNodeID;
    }
}