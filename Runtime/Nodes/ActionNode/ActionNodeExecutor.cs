using System.Threading.Tasks;

namespace SimpleDialogues.Runtime
{
    public class ActionNodeExecutor : IRuntimeNodeExecutor<RuntimeActionNode>
    {
        public async Task ExecuteAsync(RuntimeActionNode node, DialogueTreeDirector treeDirector)
        {
            node.evt.Invoke();
            await treeDirector.ProcessNode(node.nextNodeID);
        }
    }
}