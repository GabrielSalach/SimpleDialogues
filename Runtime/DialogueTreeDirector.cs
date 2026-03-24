using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleDialogues.Runtime
{
    public class DialogueTreeDirector
    {
        private readonly RuntimeDialogueTree dialogueTree;
        private readonly NodeExecutorResolver resolver;

        private Dictionary<string, RuntimeBaseNode> nodes;
        private string currentNodeID;
        
        public Action OnDialogueComplete;

        public DialogueTreeDirector(RuntimeDialogueTree dialogueTree, NodeExecutorResolver resolver)
        {
            this.dialogueTree = dialogueTree;
            this.resolver = resolver;
        }

        public async Task Start()
        {
            nodes = dialogueTree.lookUpTable.GetLookUpTable();
            await ProcessNode(dialogueTree.startingNodeID);
        }

        public async Task ProcessNode(string nodeID)
        {
            currentNodeID = nodeID;
            
            if (string.IsNullOrEmpty(nodeID))
            {
                OnDialogueComplete?.Invoke();
                return;
            }

            
            await DispatchExecutors(nodes[nodeID]);
        }

        private Task DispatchExecutors(RuntimeBaseNode node)
        {
            return node switch
            {
                RuntimeDialogueNode   n => resolver.Resolve(n).ExecuteAsync(n, this),
                RuntimeChoiceNode     n => resolver.Resolve(n).ExecuteAsync(n, this),
                RuntimeScriptableNode n => resolver.Resolve(n).ExecuteAsync(n, this),
                RuntimeActionNode     n => resolver.Resolve(n).ExecuteAsync(n, this),
                _ => throw new InvalidOperationException($"Node type doesn't have an executor: {node.GetType().Name}")
            };
        }


        // private readonly RuntimeDialogueTree _tree;
        // private readonly IDialogueDisplay dialogueDisplay;
        // public string currentNodeID {get; private set;}
        // public Action onDialogueEnd;
        //
        // public DialogueTreeDirector(RuntimeDialogueTree _tree, IDialogueDisplay dialogueDisplay)
        // {
        //     this._tree = _tree;
        //     currentNodeID = _tree.startingNodeID;
        //     this.dialogueDisplay = dialogueDisplay;
        // }
        //
        // public void ProcessNextNode()
        // {
        //     ProcessNode(currentNodeID);
        // }
        //
        // private void ProcessNode(string nodeId)
        // {
        //     if (nodeId == null)
        //     {
        //         onDialogueEnd.Invoke();
        //         return;
        //     }
        //
        //     _tree.nodes.TryGetValue(currentNodeID, out RuntimeBaseNode currentNode);
        //
        //     switch (currentNode)
        //     {
        //         case RuntimeDialogueNode dialogueNode:
        //         {
        //             dialogueDisplay.DisplayDialogue(dialogueNode.dialogueLine, (() =>
        //             {
        //                 currentNodeID = dialogueNode.nextNodeID;
        //             }));
        //             break;
        //         }
        //     }
        // }
        //
        // // public IEnumerator MainLoop()
        // // {
        // //     while (true)
        // //     {
        // //         graph.nodes.TryGetValue(currentNodeID, out RuntimeNode currentNode);
        // //
        // //         if (currentNode == null)
        // //         {
        // //             Debug.Log("Null");
        // //             onDialogueEnd.Invoke();
        // //             break;
        // //         }
        // //
        // //         switch (currentNode)
        // //         {
        // //             case RuntimeDialogueNode dialogueNode:
        // //             {
        // //                 Debug.Log("Dialogue");
        // //                 dialogueDisplay.DisplayDialogue(dialogueNode.dialogueLine, (() => {}));
        // //                 currentNodeID = dialogueNode.nextNodeID;
        // //                 yield return null;
        // //                 break;
        // //             }
        // //             case RuntimeChoiceNode choiceNode:
        // //             {
        // //                 Debug.Log("Choice");
        // //                 dialogueDisplay.DisplayChoices(choiceNode.choicesText, SetChoice);
        // //                 isWaitingForChoice = true;
        // //                 while (isWaitingForChoice)
        // //                 {
        // //                     yield return null;
        // //                 }
        // //
        // //                 currentNodeID = choiceNode.nextNodesID[choice];
        // //                 break;
        // //             }
        // //             case RuntimeScriptableNode scriptableNode:
        // //             {
        // //                 Debug.Log("Scriptable");
        // //                 //TODO : Validate if evaluate returns value < outputCount
        // //                 currentNodeID = scriptableNode.nextNodesID[scriptableNode.evaluator.Evaluate()];
        // //                 break;
        // //             }
        // //             default:
        // //             {
        // //                 Debug.LogError("Node couldn't be processed, aborting");
        // //                 onDialogueEnd.Invoke();
        // //                 break;
        // //             }
        // //         }
        // //
        // //         yield return null;
        // //     }
        // // }
        //
    }
}