using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace SimpleDialogues.Editor
{
    [Serializable]
    [Graph(AssetExtension)]
    public class DialogueTreeAsset : Graph
    {
        public const string AssetExtension = "dga";

        [MenuItem("Assets/Create/SimpleDialogues/DialogueTreeAsset")]
        static void CreateAsset()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueTreeAsset>("DialogueTreeAsset");
        }
    }
}