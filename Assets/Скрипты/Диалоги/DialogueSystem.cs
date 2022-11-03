using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Dialogues;

public class DialogueSystem : SequenceObject
{
    public RectTransform dialogueBox;
    public RectTransform variantBox;
    public DialogueVariantObject variantPrefab;
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

    [HideInInspector] public DialoguesState state;
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
    }

    public void ChangeState(DialoguesState st)
    {
        if (state != null)
        {
            StartCoroutine(state.Stop());
            InputManager.Input.Player.Submit.performed -= state.Submit;
            InputManager.Input.Player.Move.started -= state.Move;
        }

        state = st;
        InputManager.Input.Player.Submit.performed += state.Submit;
        InputManager.Input.Player.Move.started += state.Move;

        StartCoroutine(state.Start());
    }

    public static void StartDialogue(Dialogue[] dialogues, GameObject obj)
    {
        var active = Active;

        if (!(active.state is IdleState))
            return;
        
        active.StopAllCoroutines();
        active.obj = obj;
        active.dialogues = dialogues;
        active.index = 0;

        active.ChangeState(active.printingState);
    }

    public void StopDialogue() =>
        ChangeState(idleState);

    public void GUIInput() =>
        state.GUIInput();

    public void ChangeDialogue()
    {
        index++;

        if (index >= dialogues.Length)
        {
            ChangeState(idleState);
            return;
        }

        ChangeState(printingState);
    }

    public void ChangeVariant(int variant)
    {
        if (variant >= 0 && variant < current.variants.Length)
        {
            currentVariant = variant;

            for (int i = 0; i < current.variants.Length; i++)
                variantObjects[i].GetComponent<TMP_Text>().faceColor = i == currentVariant ? Color.white : Color.grey;
        }
    }

    public void ChooseVariant() 
    {
        foreach (var i in variantObjects)
            Destroy(i);

        variantObjects = new List<GameObject>();
        ChangeState(idleState);

        if (current.variants[currentVariant].action != null)
            current.variants[currentVariant].action.Invoke();

        currentVariant = 0;
    }

    public override IEnumerator Sequence()
    {
        yield return new WaitUntil(() => state is IdleState);
    }

    private void OnEnable()
    {
        StartCoroutine(state.Start());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

[Serializable]
public class Dialogue
{
    public string character;
    public DialogueCharacter dialogueCharacter;
    public DialogueCharacter.Emotion emotion;
    public Color color = Color.white;
    public float delay = .015f;
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

    public Dialogue(string character, DialogueCharacter dialogueCharacter, string text, UnityEvent<GameObject> action, 
        bool clearPreviousText, bool showStraightaway, bool dontWait, float startDelay, bool cantSkip, DialogueCharacter.Emotion emotion,
        float delay, Color color)
    {
        this.character = character;
        this.dialogueCharacter = dialogueCharacter;
        this.emotion = emotion;
        this.text = text;
        this.action = action;
        this.clearPreviousText = clearPreviousText;
        this.showStraightaway = showStraightaway;
        this.dontWait = dontWait;
        this.startDelay = startDelay;
        this.cantSkip = cantSkip;
        this.delay = delay;
        this.color = color;
    }
}
