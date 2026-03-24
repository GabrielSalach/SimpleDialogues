using System.Threading.Tasks;

namespace SimpleDialogues.Runtime
{
    public interface IRuntimeNodeExecutor<in TNode> where TNode : RuntimeBaseNode
    {
        Task ExecuteAsync(TNode node, DialogueTreeDirector treeDirector);
    }
}