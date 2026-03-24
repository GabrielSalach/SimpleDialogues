using System;
using Unity.GraphToolkit.Editor;
using UnityEngine.Events;

namespace SimpleDialogues.Editor
{
    [Serializable]
    internal class ActionNode : BaseNode
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
            
            context.AddInputPort<UnityEvent>("Action");
        }
    }
}