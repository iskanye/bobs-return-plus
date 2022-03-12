using UnityEngine;

public class RandomActions : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent<GameObject>[] action;

    GameObject obj;

    public void Action() 
    {
        int rand = Random.Range(0, action.Length);

        if (action[rand] != null)
            action[rand].Invoke(obj);
    }

    public void Action(GameObject o)
    {
        Action();
        obj = o;
    }
}
