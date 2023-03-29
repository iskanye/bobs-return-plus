using UnityEngine;
using UnityEngine.Events;

public class PropabilityActions : MonoBehaviour
{
    [System.Serializable]
    public struct Propability
    {
        public UnityEvent<GameObject> action;
        public float propability; 
    }

    public Propability[] actions;

    public void Action(GameObject obj) 
    {
        float random = Random.value;

        float prevPropability = 0, propability = 0;

        for (int i = 0; i < actions.Length; i++) 
        {
            propability += actions[i].propability;

            if (random >= prevPropability && random <= propability && actions[i].action != null)
                actions[i].action?.Invoke(obj);

            prevPropability += actions[i].propability;
        }
    }
}
