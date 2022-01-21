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
    MovementBob player;
    int index;
    bool isDialogue;
    bool isPrinting;
    bool isReady = true;

    void Update()
    {
        if (isDialogue)
        {
            if (index >= dialogues.Length)
            {
                StopDialogue();
                return;
            }

            current = dialogues[index];

            if (isReady) StartCoroutine(Print(current.text, current.character));

            firstVariant.gameObject.SetActive(current.isNonlinear && !isPrinting);
            firstVariantText.text = current.firstVariant;
            secondVariant.gameObject.SetActive(current.isNonlinear && !isPrinting);
            secondVariantText.text = current.secondVariant;

            if (Input.GetKeyDown(KeyCode.Space) && !isReady && !current.isNonlinear)
            {
                isReady = true;
                index++;
                text.text = "";
            }

            dialogueBox.transform.localScale = Vector3.Lerp(dialogueBox.transform.localScale, Vector3.one, .2f);
        }
        
        else dialogueBox.transform.localScale = Vector3.Lerp(dialogueBox.transform.localScale, Vector3.zero, .2f);
    }

    IEnumerator Print(string text, string character)
    {
        this.character.text = character;
        if (current.action != null) current.action.Invoke();
        isReady = false;
        isPrinting = true;

        foreach (var j in text)
        {
            this.text.text += j;
            yield return new WaitForFixedUpdate();
        }

        isPrinting = false;
    }

    IEnumerator SetDialogue(bool set)
    {
        yield return new WaitForEndOfFrame();
        isDialogue = set;
        if (!set) dialogues = null;
    }

    void StopDialogue()
    {
        text.text = "";
        firstVariant.gameObject.SetActive(false);
        secondVariant.gameObject.SetActive(false);
        player.enabled = true;
        StartCoroutine(SetDialogue(false));
    }

    public void StartDialogue(Dialogue[] dialogues)
    {
        if (isDialogue) return;

        this.dialogues = dialogues;
        index = 0;
        isReady = true;
        player = FindObjectOfType<MovementBob>();
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(SetDialogue(true));
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
                    text.text = "";
                    isReady = true;
                    dialogues = current.firstVariantContinuation;
                    index = 0;
                }
                break;
            case 2:
                if (current.isSecondVariantStops)
                {
                    StopDialogue();
                    current.secondVariantAction.Invoke();
                }

                else
                {
                    text.text = "";
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

    public Dialogue(string character, string text, UnityEvent action)
    {
        this.character = character;
        this.text = text;
        this.action = action;
        isNonlinear = false;
    }
}