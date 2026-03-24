using System;
using Unity.GraphToolkit.Editor;

namespace SimpleDialogues.Editor
{
    [Serializable]
    internal class DialogueNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            
            
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddInputPort<CharacterData>("Character").Build();
            context.AddInputPort<string>("Dialogue Line").Build();
            context.AddInputPort<bool>("Require Player Input").WithDefaultValue(true).Build();
        }
    }
}