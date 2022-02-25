public class SequentialEvent : SequenceObject
{
    public PartOfSequence[] sequence;

    State<SequentialEvent> currentState;
    SequenceState sequenceState;

    void Awake() =>
        sequenceState = new SequenceState(this);

    public void StartSequence() =>
        ChangeState(sequenceState);

    public void ChangeState(State<SequentialEvent> state) 
    {
        if (currentState != null)
            StartCoroutine(currentState.Stop());

        currentState = state;

        if (currentState != null)
            StartCoroutine(currentState.Start());
    }
}

[System.Serializable]
public struct PartOfSequence 
{
    public UnityEngine.Events.UnityEvent action;
    public SequenceObject sequenceObject;
}

public class SequenceState : State<SequentialEvent> 
{ 
    public SequenceState(SequentialEvent mn) : base(mn) { }

    public override System.Collections.IEnumerator Update() 
    {
        int index = 0;

        while (true)
        {
            if (mn.sequence[index].action != null)
                mn.sequence[index].action.Invoke();

            yield return base.Update();
            yield return mn.sequence[index].sequenceObject.Sequence();

            index++;

            if (index >= mn.sequence.Length)
            {
                index = 0;
                mn.ChangeState(null);
            }

            yield return base.Update();
        }
    }
}
