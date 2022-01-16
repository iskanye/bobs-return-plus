using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text character;
    public TMP_Text text;
    public Button firstVariant;
    public TMP_Text firstVariantText;
    public Button secondVariant;
    public TMP_Text secondVariantText;

    Dialogue[] dialogues;
    Dialogue current;
    int index;
    bool isDialogue;
    bool isPrinting;
    bool isReady = true;

    void Update()
    {
        if (isDialogue)
        {
            if (index >= dialogues.Length) StopDialogue();

            current = dialogues[index];
            firstVariant.gameObject.SetActive(current.isNonlinear && !isPrinting);
            firstVariantText.text = current.firstVariant;
            secondVariant.gameObject.SetActive(current.isNonlinear && !isPrinting);            
            secondVariantText.text = current.secondVariant;

            if (isReady && !isPrinting) StartCoroutine(Print(current.text, current.character));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isPrinting)
                {
                    isPrinting = false;
                    StopAllCoroutines();
                    text.text = current.text;
                }
                else if (!isReady && current.isNonlinear)
                {
                    isReady = true;
                    index++;
                }
            }
        }
    }

    IEnumerator Print(string text, string character)
    {
        this.character.text = character;
        current.action.Invoke();
        isReady = false;
        isPrinting = true;

        foreach (var j in text)
        {
            this.text.text += j;
            yield return new WaitForFixedUpdate();
        }

        isPrinting = false;
    }

    IEnumerator SetDialogue()
    {
        yield return new WaitForEndOfFrame();
        isDialogue = true;
    }

    void StopDialogue()
    {
        foreach (var i in FindObjectsOfType<Interact>()) i.enabled = true;
        FindObjectOfType<MovementBob>().enabled = true;

        dialogueBox.SetActive(false);
        isDialogue = false;
        dialogues = null;
    }

    public void StartDialogue(Dialogue[] dialogues)
    {
        this.dialogues = dialogues;
        index = 0;
        isReady = true;
        dialogueBox.SetActive(true);
        text.text = "";
        FindObjectOfType<MovementBob>().enabled = false;

        foreach (var i in FindObjectsOfType<Interact>()) i.enabled = false;

        StartCoroutine(SetDialogue());
    }

    public void ChooseVariant(int variant)
    {
        switch (variant)
        {
            case 1:
                if (current.isFirstVariantStops) 
                {
                    StopDialogue();
                    current.firstVariantAction.Invoke();
                }

                else
                {
                    isReady = true;
                    dialogues = current.firstVariantContinuation;
                    index = 0;
                }
                break;
            case 2:
                if (current.isFirstVariantStops) 
                {
                    StopDialogue();
                    current.secondVariantAction.Invoke();
                }

                else
                {
                    isReady = true;
                    dialogues = current.secondVariantContinuation;
                    index = 0;
                }
                break;
        }
    }
}
[System.Serializable]
public class Dialogue
{
    public string character;
    public string text;
    public UnityEvent action;

    public bool isNonlinear;

    public string firstVariant;
    public bool isFirstVariantStops;
    public UnityEvent firstVariantAction;
    public Dialogue[] firstVariantContinuation;

    public string secondVariant;
    public bool isSecondVariantStops;
    public UnityEvent secondVariantAction;
    public Dialogue[] secondVariantContinuation;
}
