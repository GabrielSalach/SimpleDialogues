using UnityEngine;

namespace SimpleDialogues.Runtime
{
    public abstract class ScriptableNodeEvaluator : ScriptableObject
    {
        public abstract int Evaluate();
    }
}