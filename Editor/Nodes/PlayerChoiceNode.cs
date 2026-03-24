using System;
using Unity.GraphToolkit.Editor;

namespace SimpleDialogues.Editor
{
    [Serializable]
    internal class PlayerChoiceNode : BaseNode
    {
        private const string optionName = "Choice Amount";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(optionName);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            INodeOption option = GetNodeOptionByName(optionName);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<string>($"Choice {i} text").Build();
                context.AddOutputPort($"Choice {i}")
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();
            }
        }
    }
}