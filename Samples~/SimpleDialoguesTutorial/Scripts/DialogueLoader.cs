using SimpleDialogues.Runtime;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    [SerializeField] RuntimeDialogueTree tree;
    [SerializeField] SampleDialogueDisplay sampleDialogueDisplay;

    private async void Start()
    {
        DialogueTreeDirector director = TreeDirectorInstaller.Install(tree, sampleDialogueDisplay);
        director.OnDialogueComplete = () =>
        {
            sampleDialogueDisplay.gameObject.SetActive(false);
            Application.Quit();
        };
        await director.Start();
    }
}
