using UnityEngine;
using UnityEngine.Events;

public class StateBank : MonoBehaviour
{
    public int stateCount;
    public UnityEvent action;
    public string rightOrder;
    public UnityEvent wrongOrderAction;

    string[] _rightOrder;
    int states;
    int index;

    void Awake() =>
        _rightOrder = rightOrder.Split(new char[] { ' ' });

    public void ChangeState(int state)
    {
        var byteState = 1 << state;

        if (state > stateCount || ((1 << stateCount) & states) == 1 || (states & byteState) != 0)
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

        if (states == (int)Mathf.Pow(2, stateCount - 1) - 1)
        {
            action.Invoke();
            states |= 1 << stateCount;
        }
    }
}
