using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using TMPro;
using Dialogues;

public class DialogueSystem : SequenceObject
{
    public Transform dialogueBox;
    public RectTransform variantBox;
    public GameObject variantPrefab;
    public TMP_Text character;
    public TMP_Text text;

    public static DialogueSystem Active { get; private set; }

    [HideInInspector] public Dialogue[] dialogues;
    [HideInInspector] public Dialogue current;
    [HideInInspector] public MovementBob player;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PrintingState printingState;
    [HideInInspector] public WaitingState waitingState;

    [HideInInspector] public int index;
    [HideInInspector] public int currentVariant;
    [HideInInspector] public List<GameObject> variantObjects;

    State<DialogueSystem> state;

    void Awake()
    {
        Active = this;

        player = FindObjectOfType<MovementBob>();
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

    public static void StartDialogue(Dialogue[] dialogues)
    {
        var active = Active;

        if (active.state is PrintingState || active.state is WaitingState) return;

        active.dialogues = dialogues;
        active.index = 0;

        active.ChangeState(active.printingState);
    }

    void Input(InputAction.CallbackContext c) 
    {
        if (state is WaitingState) 
        {
            if (current.isNonlinear)
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
        }
    }

    void VariantInput(InputAction.CallbackContext c) 
    {
        if (!(state is WaitingState) || !current.isNonlinear)
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
    public string text;
    public UnityEvent action;

    public bool isNonlinear;
    public bool clearPreviousText = true;

    public DialogueVariant[] variants;

    [Serializable]
    public class DialogueVariant
    {
        public string variant;
        public UnityEvent action;
    }

    public Dialogue(string character, string text, UnityEvent action, bool clearPreviousText)
    {
        this.character = character;
        this.text = text;
        this.action = action;
        this.clearPreviousText = clearPreviousText;
        isNonlinear = false;
    }
}
