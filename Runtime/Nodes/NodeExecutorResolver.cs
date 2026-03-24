using System;
using System.Collections.Generic;

namespace SimpleDialogues.Runtime
{
    public class NodeExecutorResolver
    {
        private readonly Dictionary<Type, object> _executors = new Dictionary<Type, object>();

        public void Register<TNode>(IRuntimeNodeExecutor<TNode> executor) where TNode : RuntimeBaseNode
            => _executors[typeof(TNode)] = executor;

        public IRuntimeNodeExecutor<TNode> Resolve<TNode>(TNode node) where TNode : RuntimeBaseNode
        {
            if (_executors.TryGetValue(node.GetType(), out var executor))
                return (IRuntimeNodeExecutor<TNode>)executor;

            throw new InvalidOperationException($"No executor registered for {node.GetType().Name}");
        }
    }
}