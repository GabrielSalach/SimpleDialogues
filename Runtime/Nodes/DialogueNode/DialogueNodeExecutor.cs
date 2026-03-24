using System.Threading.Tasks;

namespace SimpleDialogues.Runtime
{
    public class DialogueNodeExecutor : IRuntimeNodeExecutor<RuntimeDialogueNode>
    {
        private readonly IDialogueDisplay display;

        public DialogueNodeExecutor(IDialogueDisplay display)
        {
            this.display = display;
        }
        
        public async Task ExecuteAsync(RuntimeDialogueNode node, DialogueTreeDirector treeDirector)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            display.DisplayDialogue(node, () => tcs.SetResult(true));
            if (node.requirePlayerInput)
            {
                await tcs.Task;
            }
            
            await treeDirector.ProcessNode(node.nextNodeID);
        }
    }
}