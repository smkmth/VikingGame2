using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplayer : MonoBehaviour
{
    //roughly 46 chars in a line

    public GameObject dialogueWindow;
    public GameObject dialoguePanel;
    public GameObject speakerPanel;
    public  Image speakerImage;

    private TextMeshProUGUI dialogueContent;
    private TextMeshProUGUI speakerName;
    private List<DialogueLine> dialogueToDisplay = new List<DialogueLine>();
    public int currentDialogueIndex =0;

    public Interaction interaction;



    public bool isDisplayingDialogue;

    public void Start()
    {
        interaction = GetComponent<Interaction>();

        dialogueContent = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        speakerName = speakerPanel.GetComponentInChildren<TextMeshProUGUI>();
        dialogueWindow.SetActive(false);

        dialogueContent.text = "test text";
        speakerName.text = "test speaker";


    }

    public void DisplayDialogue(DialogueLine dialogue)
    {
        dialogueToDisplay.Clear();
        if (!isDisplayingDialogue)
        {
            Time.timeScale = 0;
            dialogueWindow.SetActive(true);
            isDisplayingDialogue = true;
            currentDialogueIndex = 0;

            dialogueContent.text = dialogue.lineToSay;
            speakerName.text = dialogue.speakerName;
            speakerImage.sprite = dialogue.speakerImage;
        }

    }
    public void FinishDisplayingDialogue()
    {

        Time.timeScale = 1;
        dialogueWindow.SetActive(false);
        isDisplayingDialogue = false;
        interaction.canInteract = true;


    }
    public void Update()
    {
        if (isDisplayingDialogue)
        {
            interaction.canInteract = false;

            if (Input.GetButtonDown("Attack"))
            {
                if (currentDialogueIndex < dialogueToDisplay.Count)
                {

                    currentDialogueIndex++;

                }
                else
                {
                    Debug.Log("Next");
                    FinishDisplayingDialogue();
                }
            }
        }

    }


    public void ToggleWindow()
    {
        if (dialogueWindow.activeSelf)
        {

            dialogueWindow.SetActive(false);

        }
        else
        {

            dialogueWindow.SetActive(true);
        }
    }

}
