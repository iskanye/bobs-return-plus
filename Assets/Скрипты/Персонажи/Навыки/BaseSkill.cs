using UnityEngine;
using System.Collections;

public class BaseSkill : ScriptableObject
{
    [HideInInspector] public BaseBob bob;

    public virtual IEnumerator Start() 
    {
        yield return null;
    }

    public virtual IEnumerator Process() 
    {
        yield return null;
    }
}
