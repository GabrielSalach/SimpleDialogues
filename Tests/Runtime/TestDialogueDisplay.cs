using System;
using System.Collections.Generic;
using SimpleDialogues.Runtime;
using UnityEngine;

public class TestDialogueDisplay : IDialogueDisplay
{
    private readonly int choicePreset;
    
    public Action<string> OnDialogueLine;
    public Action<string> OnPlayerChoice;

    public TestDialogueDisplay(int choicePreset = 0)
    {
        this.choicePreset = choicePreset;
    }
    
    public void DisplayDialogue(RuntimeDialogueNode node, IDialogueDisplay.OnContinueInput onContinueInput)
    {
        OnDialogueLine?.Invoke(node.dialogueLine);
        onContinueInput?.Invoke();
    }

    public void DisplayChoices(List<string> choices, IDialogueDisplay.OnChoiceSelected onChoiceSelected)
    {
        foreach (string t in choices)
        {
            OnPlayerChoice?.Invoke(t);
        }

        onChoiceSelected?.Invoke(choicePreset);
    }
}
