using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using TMPro;
using Dialogues;

public class DialogueSystem : SequenceObject
{
    public RectTransform dialogueBox;
    public RectTransform variantBox;
    public GameObject variantPrefab;
    public UnityEngine.UI.Image novelSprite;
    public RectTransform novelTransform;
    public TMP_Text character;
    public TMP_Text text;
    public AudioSource textSFX;

    public static DialogueSystem Active { get; private set; }

    [HideInInspector] public Dialogue[] dialogues;
    [HideInInspector] public Dialogue current;
    [HideInInspector] public TopDownMovement player;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PrintingState printingState;
    [HideInInspector] public WaitingState waitingState;

    [HideInInspector] public int index;
    [HideInInspector] public int currentVariant;
    [HideInInspector] public List<GameObject> variantObjects;

    [HideInInspector] public State<DialogueSystem> state;
    [HideInInspector] public GameObject obj;
    [HideInInspector] public string prevText;

    void Awake()
    {
        Active = this;

        player = FindObjectOfType<TopDownMovement>();
        variantObjects = new List<GameObject>();

        idleState = new IdleState(this);
        printingState = new PrintingState(this);
        waitingState = new WaitingState(this);

        ChangeState(idleState);

        InputManager.Input.Player.Submit.canceled += Input;
        InputManager.Input.Player.Move.started += VariantInput;
    }

    public void ChangeState(State<DialogueSystem> st)
    {  
        if (state != null)
            StartCoroutine(state.Stop());

        state = st;
        StartCoroutine(state.Start());
    }

    public static void StartDialogue(Dialogue[] dialogues, GameObject obj)
    {
        var active = Active;

        if (active.state is PrintingState || active.state is WaitingState) 
            return;

        active.obj = obj;
        active.dialogues = dialogues;
        active.index = 0;

        active.ChangeState(active.printingState);
    }

    public void StopDialogue() =>
        ChangeState(idleState);

    public void GUIInput()
    {
        if (current.variants != null)
            return;

        if (state is WaitingState)
        {
            index++;

            if (index >= dialogues.Length)
            {
                ChangeState(idleState);
                return;
            }

            ChangeState(printingState);
            return;
        }

        if (state is PrintingState) 
        {
            ChangeState(waitingState);
            text.text = prevText + current.text;
        }
    }

    void Input(InputAction.CallbackContext c) 
    {
        if (state is WaitingState) 
        {
            if (current.variants != null)
            {
                foreach (var i in variantObjects)
                    Destroy(i);

                variantObjects = new List<GameObject>();
                ChangeState(idleState);

                if (current.variants[currentVariant].action != null)
                    current.variants[currentVariant].action.Invoke();

                currentVariant = 0;
                return;
            }

            index++;

            if (index >= dialogues.Length)
            {
                ChangeState(idleState);
                return;
            }

            ChangeState(printingState);
            return;
        }

        if (!current.cantSkip && state is PrintingState)
        {
            text.text = prevText + current.text;
            ChangeState(waitingState);
        }
    }

    void VariantInput(InputAction.CallbackContext c) 
    {
        if (!(state is WaitingState) || current.variants == null)
            return;

        var val = c.ReadValue<Vector2>();
        var variant = currentVariant + (val.y > 0 ? -1 : 1);

        if (variant >= 0 && variant < current.variants.Length)
        {
            currentVariant = variant;

            for (int i = 0; i < current.variants.Length; i++) 
                variantObjects[i].GetComponent<TMP_Text>().faceColor = i == currentVariant ? Color.white : Color.grey;
        }
    }

    public override System.Collections.IEnumerator Sequence()
    {
        yield return new WaitUntil(() => state is IdleState);
    }
}

[Serializable]
public class Dialogue
{
    public string character;
    public Sprite characterSprite;
    [TextArea] public string text;
    public UnityEvent<GameObject> action;

    public float startDelay;

    public bool clearPreviousText = true;
    public bool showStraightaway;
    public bool dontWait;
    public bool cantSkip;

    public DialogueVariant[] variants;

    [Serializable]
    public class DialogueVariant
    {
        public string variant;
        public UnityEvent action;
    }

    public Dialogue(string character, Sprite characterSprite, string text, UnityEvent<GameObject> action, 
        bool clearPreviousText, bool showStraightaway, bool dontWait, float startDelay, bool cantSkip)
    {
        this.character = character;
        this.characterSprite = characterSprite;
        this.text = text;
        this.action = action;
        this.clearPreviousText = clearPreviousText;
        this.showStraightaway = showStraightaway;
        this.dontWait = dontWait;
        this.startDelay = startDelay;
        this.cantSkip = cantSkip;
    }
}
