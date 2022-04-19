using UnityEngine;

public class PropabilityActions : MonoBehaviour
{
    [System.Serializable]
    public struct Propability
    {
        public UnityEngine.Events.UnityAction<GameObject> action;
        public int propability; 
    }

    public Propability[] actions;

    GameObject obj;

    public void Action() 
    {
        int rand = Random.Range(0, 101);

        int prevProp = 0;
        int prop = actions[0].propability;

        for (int i = 0; i < actions.Length; i++) 
        {
            prop += actions[i].propability;

            if (rand >= prevProp && rand <= prop && actions[i].action != null)
                actions[i].action.Invoke(obj);

            prevProp += actions[i].propability;
        }
    }

    public void Action(GameObject obj) 
    {
        this.obj = obj;
        Action(); 
    }
}
