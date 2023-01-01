using UnityEngine;

public class PropabilityActions : MonoBehaviour
{
    [System.Serializable]
    public struct Propability
    {
        public UnityEngine.Events.UnityEvent<GameObject> action;
        public float propability; 
    }

    public Propability[] actions;

    GameObject obj;

    public void Action() 
    {
        float random = Random.value;

        float prevPropability = 0, propability = 0;

        for (int i = 0; i < actions.Length; i++) 
        {
            propability += actions[i].propability;

            if (random >= prevPropability && random <= propability && actions[i].action != null)
                actions[i].action.Invoke(obj);

            prevPropability += actions[i].propability;
        }
    }

    public void Action(GameObject obj) 
    {
        this.obj = obj;
        Action(); 
    }
}
