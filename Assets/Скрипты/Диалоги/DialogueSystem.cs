using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Dialogues;

public class DialogueSystem : MonoBehaviour, IInputListener
{
    public GameObject dialogueBox;
    public TMP_Text character;
    public TMP_Text text;
    public Button firstVariant;
    public TMP_Text firstVariantText;
    public Button secondVariant;
    public TMP_Text secondVariantText;

    public static DialogueSystem Active { get; private set; }

    [HideInInspector] public Dialogue[] dialogues;
    [HideInInspector] public Dialogue current;
    [HideInInspector] public MovementBob player;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PrintingState printingState;
    [HideInInspector] public WaitingState waitingState;

    [HideInInspector] public int index;

    State<DialogueSystem> state;

    void Awake()
    {
        Active = this;
        player = FindObjectOfType<MovementBob>();

        idleState = new IdleState(this);
        printingState = new PrintingState(this);
        waitingState = new WaitingState(this);

        ChangeState(idleState);
    }

    void Start() =>
        InputManager.AddListenerToActionCanceled("Submit", Input);

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

    public void ChooseVariant(int variant)
    {
        switch (variant)
        {
            case 1:
                if (current.isFirstVariantStops)
                {
                    ChangeState(idleState);
                    current.firstVariantAction.Invoke();
                }

                else
                {
                    dialogues = current.firstVariantContinuation;
                    index = 0;
                    ChangeState(printingState);
                }
                break;

            case 2:
                if (current.isSecondVariantStops)
                {
                    ChangeState(idleState);
                    current.secondVariantAction.Invoke();
                }

                else
                {
                    dialogues = current.secondVariantContinuation;
                    index = 0;
                    ChangeState(printingState);
                }
                break;
        }
    }

    public void Input(UnityEngine.InputSystem.InputAction.CallbackContext c) 
    {
        if (current.isNonlinear) return;

        if (state is WaitingState) 
        {
            index++;
            if (index >= dialogues.Length)
            {
                ChangeState(idleState);
                return;
            }
            ChangeState(printingState);
        }
    }

    public void DeleteListeners() =>
        InputManager.RemoveListenerAtActionCanceled("Submit", Input);
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
