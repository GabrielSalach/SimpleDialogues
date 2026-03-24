using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleDialogues.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class SampleDialogueDisplay : MonoBehaviour, IDialogueDisplay
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    private Label dialogueLabel;
    private VisualElement userPromptIcon;
    private VisualElement choicesContainer;

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        dialogueLabel = root.Q<Label>("DialogueLine");
        userPromptIcon = root.Q<VisualElement>("ContinuePrompt");
        choicesContainer = root.Q("ChoicesContainer");
    }
    

    public void DisplayDialogue(RuntimeDialogueNode node, IDialogueDisplay.OnContinueInput onContinueInput)
    {
        dialogueLabel.text = node.dialogueLine;
        if (node.requirePlayerInput)
        {
            userPromptIcon.style.display = DisplayStyle.Flex;
            SetContinueCallback(onContinueInput);
        }
    }

    public void DisplayChoices(List<string> choices, IDialogueDisplay.OnChoiceSelected onChoiceSelected)
    {
        for(int i = 0; i < choices.Count; i++)
        {
            Label choiceLabel = new Label(choices[i]);
            choiceLabel.AddToClassList("choiceText");
            choicesContainer.Add(choiceLabel);
            SetChoiceCallback(i, onChoiceSelected);
        }
        choicesContainer.style.display = DisplayStyle.Flex;
    }

    private void SetContinueCallback(IDialogueDisplay.OnContinueInput onContinueInput)
    {
        root.RegisterCallbackOnce<MouseDownEvent>(_evt =>
        {
            userPromptIcon.style.display = DisplayStyle.None;
            onContinueInput?.Invoke();
        }, TrickleDown.TrickleDown);
    }

    private void SetChoiceCallback(int choiceIndex, IDialogueDisplay.OnChoiceSelected onChoiceSelected)
    {
        choicesContainer[choiceIndex].RegisterCallbackOnce<MouseDownEvent>(_evt =>
        {
            choicesContainer.Clear();
            choicesContainer.style.display = DisplayStyle.None;
            onChoiceSelected?.Invoke(choiceIndex);
        }, TrickleDown.TrickleDown);
    }
}
