using UnityEngine;

public class RandomActions : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent[] action;

    public void Action() 
    {
        int rand = Random.Range(0, action.Length);

        if (action[rand] != null)
            action[rand].Invoke();
    }
}
