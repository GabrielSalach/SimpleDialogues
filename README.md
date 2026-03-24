# SimpleDialogues

SimpleDialogues provides an easy way to edit and run dialogue in Unity. 
It is built on top of the newly released GraphToolkit package and was created with simplicity and flexibility in mind.

Please be mindful that GraphToolkit is still an experimental package and that the API is currently pretty limited, making this tool not ready for production yet.
I really hope Unity develops it further (especially for serialization of custom types), and I'll be updating this project as new updates come.

## How does it work ? 
SimpleDialogues is built around 3 main types of nodes :

#### Dialogue Node
This node is used to send a line of text. You can provide information about the character saying this line through a scriptable object, and whether or not the player needs to press a key to continue (on by default).

#### Choice Node
A choice node pauses the game and sends the choices you provided to the display. Once the player selects their answer, the dialogue follows through the corresponding path. 

#### Scriptable Node
This is where this tool's flexibility comes in. Scriptable nodes provide a way to inject your own custom logic into the dialogue via a scriptable object. After defining a maximum number of output for the node, you need to create a new class inheriting from `ScriptableNodeEvaluator`.
```c#
using SimpleDialogues.Runtime;

public class MyCustomScriptableNode : ScriptableNodeEvaluator
{
    public override int Evaluate()
    {
        // My custom logic...
    }
}
```
The return value of the `Evaluate()` method defines the output port's index.

## How to set it up ?

First, you'll need to import the package by going into Window -> Package Management -> Package Manager, then clicking the + symbol -> Install package from git URL, and paste this repository's URL in the popup. 

Next, you'll need to create your DialogueTreeAsset, by right-clicking in your project, then clicking Create -> SimpleDialogues -> DialogueTreeAsset.
This newly created asset will open the nodes editor.

Once you're done editing, you'll need to create a `IDialogueDisplay` implementation.

```c#
using System.Collections.Generic;
using SimpleDialogues.Runtime;

public class MyDialogueDisplay : IDialogueDisplay
{
    public void DisplayDialogue(RuntimeDialogueNode node, IDialogueDisplay.OnContinueInput onContinueInput)
    {
        // Displays the dialogue line...
    }

    public void DisplayChoices(List<string> choices, IDialogueDisplay.OnChoiceSelected onChoiceSelected)
    {
        // Prompts the player with choices...
    }
}
```

Link up this class to your UI and input system. 

`DisplayDialogues()` gives you access to all the information you defined on your dialogue node. If the `Requires Player Input` parameter is set to true, the `onContinueInput` callback must be called for the state machine to step forward. 

`DisplayChoices()` works similarly, instead giving you a list of strings for the choices, with the callback to invoke with the desired index passed in.

Now, all you need to do is to create a new `DialogueTreeDirector` and start it. 
```c#
async void StartDialogue()
{
    DialogueTreeDirector director = TreeDirectorInstaller.Install(dialogueTreeAsset, MyDialogueDisplay);
    director.OnDialogueComplete = () =>
    {
        // Close the dialogue box UI...
    };
    await director.Start();
}
```

You can find concrete examples of these implementations in the package's samples. 

## Known limitations
#### 1. It's only for static text 
The system currently uses raw strings. If the player can change their character's name for example, there's no way to pass that variable into a DialogueNode. Implementing this whilst keeping the simplicity of this tool intact would require a deeper control over GraphToolkit's serialization process that we don't have at the moment. 

#### 2. Scriptable Nodes can't have acces to runtime values (unless you know how to do it...)
The scriptable object nature of the `ScriptableNodeEvaluator` makes it not suited to hold references to runtime objects (i.e. stuff from your scene). If you need to use a reference to a runtime object, you should get it directly in the `int Evaluate()` method using a singleton or a service locator pattern/register etc...

## Future development
As stated above, I'll keep updating this tool if GraphToolkit gets new interesting features that fits this project. 
I also still have some refactoring and renaming to do, as well as completing the documentation. 

## References
- https://docs.unity3d.com/Packages/com.unity.graphtoolkit@0.4/manual/introduction.html
- https://www.youtube.com/watch?v=3WKW6bRlt84
- https://www.youtube.com/watch?v=Spa8au6cOmo