using System.Threading.Tasks;

namespace SimpleDialogues.Runtime
{
    public class ScriptableNodeExecutor : IRuntimeNodeExecutor<RuntimeScriptableNode>
    {
        public async Task ExecuteAsync(RuntimeScriptableNode node, DialogueTreeDirector treeDirector)
        {
            int result = node.evaluator.Evaluate();
            await treeDirector.ProcessNode(node.nextNodesID[result]);
        }
    }
}