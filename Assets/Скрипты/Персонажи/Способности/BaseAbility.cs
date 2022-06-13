using UnityEngine;

public class BaseAbility : ScriptableObject
{
    [HideInInspector] public BaseBob bob;

    public virtual System.Collections.IEnumerator Ability() 
    {
        yield return null;
    }
}
