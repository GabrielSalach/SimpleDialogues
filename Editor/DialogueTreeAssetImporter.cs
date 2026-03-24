using System;
using System.Collections.Generic;
using System.Linq;
using SimpleDialogues.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialogues.Editor
{
    [ScriptedImporter(1, DialogueTreeAsset.AssetExtension)]
    public class DialogueTreeAssetImporter : ScriptedImporter
    {
        Dictionary<INode, string> nodeIDMap;
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            DialogueTreeAsset graph = GraphDatabase.LoadGraphForImporter<DialogueTreeAsset>(ctx.assetPath);
            RuntimeDialogueTree runtimeTree = ScriptableObject.CreateInstance<RuntimeDialogueTree>();


            
            // Get new GUIDs for every node and set node count
            nodeIDMap = new Dictionary<INode, string>();
            foreach (INode node in graph.GetNodes())
            {
                nodeIDMap[node] = Guid.NewGuid().ToString();
            }
            
            runtimeTree.nodeCount = nodeIDMap.Count;

            // Sets the start node in the runtime graph
            INode startNode = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNode != null)
            {
                IPort entryPort = startNode.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
                if (entryPort != null)
                {
                    runtimeTree.startingNodeID = nodeIDMap[entryPort.GetNode()];
                }
            }

            // Populates the runtime node list
            foreach (INode node in graph.GetNodes())
            {
                switch (node)
                {
                    case StartNode or EndNode:
                        continue;
                    case DialogueNode:
                        runtimeTree.lookUpTable.AddNode(nodeIDMap[node], ProcessDialogue(graph, node));
                        break;
                    case PlayerChoiceNode:
                        runtimeTree.lookUpTable.AddNode(nodeIDMap[node], ProcessChoice(graph, node));
                        break;
                    case ScriptableNode:
                        runtimeTree.lookUpTable.AddNode(nodeIDMap[node], ProcessScriptable(graph, node));
                        break;
                    case ActionNode:
                        runtimeTree.lookUpTable.AddNode(nodeIDMap[node], ProcessActionNode(graph, node));
                        break;
                    default:
                        Debug.LogError($"Couldn't process node of type {node.GetType()}");
                        break;
                }
            }
            
            ctx.AddObjectToAsset("RuntimeData", runtimeTree);
            ctx.SetMainObject(runtimeTree);
        }

        
        private T GetPortValue<T>(IPort port)
        {
            if(port == null) return default(T);

            if (port.isConnected)
            {
                if (port.firstConnectedPort.GetNode() is IVariableNode variableNode)
                {
                    variableNode.variable.TryGetDefaultValue(out T value);
                    return value;
                }
            }

            port.TryGetValue(out T fallbackValue);
            return fallbackValue;
        }

        private RuntimeDialogueNode ProcessDialogue(DialogueTreeAsset graph, INode node)
        {
            string dialogue = GetPortValue<string>(node.GetInputPortByName("Dialogue Line"));
            string nextNodeID = string.Empty;
            bool requirePlayerInput = GetPortValue<bool>(node.GetInputPortByName("Require Player Input"));
            
            IPort entryPort = node.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
            if (entryPort != null)
            {
                nextNodeID = nodeIDMap[entryPort.GetNode()];
            }
            
            return new RuntimeDialogueNode
            {
                nodeID = nodeIDMap[node],
                dialogueLine = dialogue,
                nextNodeID = nextNodeID,
                requirePlayerInput = requirePlayerInput
            };
        }

        private RuntimeChoiceNode ProcessChoice(DialogueTreeAsset graph, INode node)
        {
            List<string> choices = new List<string>();
            List<string> nextNodesIDs = new List<string>();

            int choicesCount = node.outputPortCount;
            for (int i = 0; i < choicesCount; i++)
            {
                choices.Add(GetPortValue<string>(node.GetInputPort(i+1)));
                
                string nextNodeID = string.Empty;
                IPort nextNodePort = node.GetOutputPort(i).firstConnectedPort;
                if (nextNodePort != null)
                {
                    nextNodeID = nodeIDMap[nextNodePort.GetNode()];
                }
                nextNodesIDs.Add(nextNodeID);
            }

            return new RuntimeChoiceNode
            {
                nodeID = nodeIDMap[node],
                choicesText = choices,
                nextNodesID = nextNodesIDs
            };
        }

        private RuntimeScriptableNode ProcessScriptable(DialogueTreeAsset graph, INode node)
        {
            List<string> nextNodesIDs = new List<string>();
            
            ScriptableNodeEvaluator scriptableEvaluator = GetPortValue<ScriptableNodeEvaluator>(node.GetInputPortByName("Evaluator"));
            

            for (int i = 0; i < node.outputPortCount; i++)
            {
                string nextNodeID = string.Empty;
                IPort nextNodePort = node.GetOutputPort(i).firstConnectedPort;
                if (nextNodePort != null)
                {
                    nextNodeID = nodeIDMap[nextNodePort.GetNode()];
                }
                nextNodesIDs.Add(nextNodeID); 
            }
            
            return new RuntimeScriptableNode
            {
                nodeID = nodeIDMap[node],
                evaluator = scriptableEvaluator,
                nextNodesID = nextNodesIDs
            }; 
        }

        private RuntimeActionNode ProcessActionNode(DialogueTreeAsset graph, INode node)
        {
            UnityEvent evt = GetPortValue<UnityEvent>(node.GetInputPortByName("Action"));
            string nextNodeID = string.Empty;
            
            IPort entryPort = node.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
            if (entryPort != null)
            {
                nextNodeID = nodeIDMap[entryPort.GetNode()];
            }
            
            return new RuntimeActionNode
            {
                nodeID = nodeIDMap[node],
                evt = evt,
                nextNodeID = nextNodeID,
            };
        }
    }
}