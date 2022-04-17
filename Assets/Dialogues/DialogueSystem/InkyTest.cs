using Ink.Runtime;
using UnityEngine;

public class InkyTest : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    private Story currentStory;
    private void Start()
    {
        currentStory = new Story(dialogue.text);
        while (currentStory.canContinue)
        {
            print(currentStory.Continue());
            currentStory.currentTags.ForEachAction(print);
            print("-------------");
        }
    }
}
