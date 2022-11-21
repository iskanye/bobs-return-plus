using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "TimeTravelBob", menuName = "Bobs/TimeTravelBob", order = 0)]
public class TimeTravelBob : MeleeBob
{
    public bool HasAxe { set; get; }

    public override IEnumerator Process()
    {
        while (true)
        {
            if (HasAxe)
                yield return melee.Process();
                
            yield return null;
        }
    }
}
