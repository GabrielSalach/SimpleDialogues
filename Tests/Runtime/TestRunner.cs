using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleDialogues.Runtime;
using NUnit.Framework;
using UnityEngine;

namespace DialogueSystem.Tests.Runtime
{
    public class TestRunner
    {

        // Helpers 
        
        private static RuntimeDialogueTree BuildTree(string startId, params RuntimeBaseNode[] nodes)
        {
            RuntimeDialogueTree tree = ScriptableObject.CreateInstance<RuntimeDialogueTree>();
            tree.startingNodeID = startId;
            tree.lookUpTable = new RuntimeNodeLUT();
            foreach (RuntimeBaseNode node in nodes)
            {
                tree.lookUpTable.AddNode(node.nodeID, node);
            }
            return tree;
        }

        private static RuntimeDialogueNode DialogueNode(string nodeId, string dialogueLine, string nextId = null)
        {
            return new RuntimeDialogueNode
            {
                nodeID = nodeId,
                dialogueLine = dialogueLine,
                nextNodeID = nextId,
            };
        }

        // Dialogue Nodes
        
        [Test]
        public async Task DialogueNode_NoPlayerInput()
        {
            List<string> lines = new List<string>();
            const string nodeId = "DialogueNodeID";
            const string dialogueLine = "TestDialogueLine";
            
            RuntimeDialogueTree tree = BuildTree(nodeId, DialogueNode(nodeId, dialogueLine));
            
            TestDialogueDisplay display = new TestDialogueDisplay();
            display.OnDialogueLine += _s =>
            {
                lines.Add(_s);
            };

            DialogueTreeDirector director = TreeDirectorInstaller.Install(tree, display);
            await director.Start();
            
            
            Assert.AreEqual(lines, new [] { dialogueLine });
        }
        
        [Test]
        public async Task DialogueNode_WithPlayerInput()
        {
            List<string> lines = new List<string>();
            const string nodeId = "DialogueNodeID";
            const string dialogueLine = "TestDialogueLine";
            
            RuntimeDialogueNode node =  DialogueNode(nodeId, dialogueLine);
            node.requirePlayerInput = true;
            RuntimeDialogueTree tree = BuildTree(nodeId, node);
            
            TestDialogueDisplay display = new TestDialogueDisplay();
            display.OnDialogueLine += _s =>
            {
                lines.Add(_s);
            };

            DialogueTreeDirector director = TreeDirectorInstaller.Install(tree, display);
            await director.Start();
            
            
            Assert.AreEqual(lines, new [] { dialogueLine });
        }
        
        [Test]
        public async Task  DialogueNode_Multiple()
        {
            List<string> lines = new List<string>();
            
            const string nodeId1 = "DialogueNodeID1";
            const string nodeId2 = "DialogueNodeID2";
            const string nodeId3 = "DialogueNodeID3";
            
            const string dialogueLine1 = "TestDialogueLine1";
            const string dialogueLine2 = "TestDialogueLine2";
            const string dialogueLine3 = "TestDialogueLine3";
            
            RuntimeDialogueTree tree = BuildTree(
                nodeId1, 
                DialogueNode(nodeId1, dialogueLine1, nodeId2),
                DialogueNode(nodeId2, dialogueLine2, nodeId3),
                DialogueNode(nodeId3, dialogueLine3));
            
            TestDialogueDisplay display = new TestDialogueDisplay();
            display.OnDialogueLine += _s =>
            {
                lines.Add(_s);
            };
            
            DialogueTreeDirector director = TreeDirectorInstaller.Install(tree, display);
            await director.Start();
            
            Assert.AreEqual(lines, new [] { dialogueLine1, dialogueLine2, dialogueLine3 });
        }
        
        [Test]
        public async Task DirectorFiresEndEvent()
        {
            List<string> lines = new List<string>();
            const string nodeId = "DialogueNodeID";
            const string dialogueLine = "TestDialogueLine";
            bool endReached = false;
            
            RuntimeDialogueTree tree = BuildTree(nodeId, DialogueNode(nodeId, dialogueLine));
            
            TestDialogueDisplay display = new TestDialogueDisplay();
            
            DialogueTreeDirector director = TreeDirectorInstaller.Install(tree, display);
            director.OnDialogueComplete += () =>  endReached = true;
            
            await director.Start();
            
            Assert.That(endReached, Is.True);
        }
        
        // Choice node 
        [Test]
        public async Task ChoiceNode_DisplaysAllChoices()
        {
            List<string> choices = new List<string>();
            const string nodeId = "ChoiceNodeID";
            const string Choice1 = "Choice1";
            const string Choice2 = "Choice2";
            const string Choice3 = "Choice3";

            RuntimeDialogueTree dialogueTree = BuildTree(nodeId, new RuntimeChoiceNode
            {
                nodeID = nodeId,

                choicesText = new List<string> { Choice1, Choice2, Choice3 },
                nextNodesID = new List<string> { "", "", "" }
            });
            
            TestDialogueDisplay display = new TestDialogueDisplay();
            DialogueTreeDirector director = TreeDirectorInstaller.Install(dialogueTree, display);
            display.OnPlayerChoice += _s =>
            {
                choices.Add(_s);
            };
            
            await director.Start();
            
            Assert.AreEqual(choices, new [] { Choice1, Choice2, Choice3 });
        }

        [Test]
        public async Task ChoiceNode_CorrectBranching()
        {
            List<string> choices = new List<string>();
            const string nodeId = "ChoiceNodeID";
            
            const string Choice1 = "Choice1";
            const string Choice2 = "Choice2";
            const string Choice3 = "Choice3";
            
            const string dialogueNode1 = "DialogueNode1";
            const string dialogueNode2 = "DialogueNode2";
            const string dialogueNode3 = "DialogueNode3";

            RuntimeDialogueTree dialogueTree = BuildTree(nodeId, 
                new RuntimeChoiceNode
                {
                    nodeID = nodeId,

                    choicesText = new List<string> { Choice1, Choice2, Choice3 },
                    nextNodesID = new List<string> { dialogueNode1, dialogueNode2, dialogueNode3 },
                },
                DialogueNode(dialogueNode1, Choice1),
                DialogueNode(dialogueNode2, Choice2),
                DialogueNode(dialogueNode3, Choice3)
                );
            
            TestDialogueDisplay display = new TestDialogueDisplay(1);
            DialogueTreeDirector director = TreeDirectorInstaller.Install(dialogueTree, display);

            display.OnDialogueLine += _s =>
            {
                choices.Add(_s);
            };
            
            await director.Start();
            
            Assert.AreEqual(choices, new [] { Choice2 });
        }
    }
}