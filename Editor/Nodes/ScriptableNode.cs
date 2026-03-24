using System;
using SimpleDialogues.Runtime;
using Unity.GraphToolkit.Editor;

namespace SimpleDialogues.Editor
{
    
    [Serializable]
    internal class ScriptableNode : BaseNode
    {
        private const string countOptionName = "Output Count";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(countOptionName);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddInputPort<ScriptableNodeEvaluator>("Evaluator");
            
            INodeOption option = GetNodeOptionByName(countOptionName);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddOutputPort($"Output {i}")
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();
            }
        }
        
        
    }
}