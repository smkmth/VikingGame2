using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[System.Serializable]
public class DialogueContainer : MonoBehaviour
{
    public List<DialogueLine> dialogue;

    [SerializeField]
    public TextAsset story;

    private InkDisplayer dialogueDisplayer;
    private TimeManager time;

    public void Start()
    {
        dialogueDisplayer = GameObject.Find("SceneManager").GetComponent<InkDisplayer>();
        time = GameObject.Find("SceneManager").GetComponent<TimeManager>();

    }

    public void Talk()
    {
        dialogueDisplayer.SetStoryFromPoint(story, "talk", time.currentHour);
    }
}


public struct StoryInfo
{


}
