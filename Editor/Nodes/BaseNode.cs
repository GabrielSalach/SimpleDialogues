using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace SimpleDialogues.Editor
{
    [Serializable]
    public abstract class BaseNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
    }
}