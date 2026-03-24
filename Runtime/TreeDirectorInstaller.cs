using System.ComponentModel;

namespace SimpleDialogues.Runtime
{
    public static class TreeDirectorInstaller
    {
        public static DialogueTreeDirector Install(RuntimeDialogueTree tree, IDialogueDisplay display)
        {
            NodeExecutorResolver resolver = new NodeExecutorResolver();
            resolver.Register(new DialogueNodeExecutor(display));
            resolver.Register(new ChoiceNodeExecutor(display));
            resolver.Register(new ScriptableNodeExecutor());
            resolver.Register(new ActionNodeExecutor());

            return new DialogueTreeDirector(tree, resolver);
        }
    }
}