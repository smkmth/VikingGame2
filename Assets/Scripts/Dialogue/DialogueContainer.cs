using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[System.Serializable]
public class DialogueContainer : MonoBehaviour
{
    public List<DialogueLine> dialogue;

    [SerializeField]
    public TextAsset file;
}
