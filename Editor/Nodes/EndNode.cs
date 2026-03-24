using System;
using Unity.GraphToolkit.Editor;

namespace SimpleDialogues.Editor
{
    [Serializable]
    internal class EndNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}