using System.Collections.Generic;

namespace SimpleDialogues.Runtime
{
    
    public interface IDialogueDisplay
    {
        public delegate void OnContinueInput();
        public delegate void OnChoiceSelected(int choice);

        public void DisplayDialogue(RuntimeDialogueNode node, OnContinueInput onContinueInput);

        public void DisplayChoices(List<string> choices, OnChoiceSelected onChoiceSelected);
    }
}