using SimpleDialogues.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSphereColor", menuName = "SimpleDialogues/Samples/ChangeSphereColor")]
public class ChangeSphereColor : ScriptableNodeEvaluator
{
    public override int Evaluate()
    {
        SphereController.instance.ChangeColor();
        return 0;
    }
}
