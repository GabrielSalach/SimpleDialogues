using System.Threading.Tasks;

namespace SimpleDialogues.Runtime
{
    public class ChoiceNodeExecutor : IRuntimeNodeExecutor<RuntimeChoiceNode>
    {
        private readonly IDialogueDisplay display;
        
        public ChoiceNodeExecutor(IDialogueDisplay display)
        {
            this.display = display;
        }
        
        public async Task ExecuteAsync(RuntimeChoiceNode node, DialogueTreeDirector treeDirector)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            display.DisplayChoices(node.choicesText, _choice => tcs.SetResult(_choice));
            int result = await tcs.Task;
            await treeDirector.ProcessNode(node.nextNodesID[result]);
        }
    }
}