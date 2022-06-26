using UnityEngine;
using UnityEngine.Events;

public class StateBank : MonoBehaviour
{
    public int stateCount;
    public UnityEvent action;
    public string rightOrder;
    public UnityEvent wrongOrderAction;

    string[] _rightOrder;
    ulong states;
    int index;

    void Awake() =>
        _rightOrder = rightOrder.Split(new char[] { ' ' });

    public void ChangeState(int state)
    {
        ulong byteState = 1UL << state;

        if (state > stateCount || ((1UL << stateCount) & states) == 1 || (states & byteState) != 0)
            return;

        if (rightOrder != null && _rightOrder[index] != state.ToString())
        {
            if (wrongOrderAction != null)
                wrongOrderAction.Invoke();

            index = 0;
            states = 0;
            return;
        }

        states |= byteState;
        index++;

        if (states == (ulong)Mathf.Pow(2, stateCount) - 1)
        {
            action.Invoke();
            states |= 1UL << stateCount;
        }
    }
}
