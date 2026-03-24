using SimpleDialogues.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "SphereStatus", menuName = "SimpleDialogues/Samples/SphereStatus")]
public class SphereStatus : ScriptableNodeEvaluator
{
    
    public override int Evaluate()
    {
        return SphereController.instance.IsBlue ? 0 : 1;
    }
}
