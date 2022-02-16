using UnityEngine;

public class StateBank : MonoBehaviour
{
    public int stateCount;
    public UnityEngine.Events.UnityEvent action;

    int states; 

    public void ChangeState(int state) 
    {
        if (state > stateCount || ((1 << stateCount) & states) == 1)
            return;

        var byteState = 1 << state;

        if ((states & byteState) == 0)
            states |= byteState;

        else
            states &= ~byteState;

        if (states == (int)Mathf.Pow(2, stateCount - 1) - 1)
        {
            action.Invoke();
            states |= 1 << stateCount;
        }
    }
}
