using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeTravelBob", menuName = "Bobs/TimeTravelBob", order = 0)]
public class TimeTravelBob : MeleeBob
{
    public bool HasAxe;

    public override IEnumerator Start()
    {
        HasAxe = false;
        yield return base.Start();
    }

    public override IEnumerator Process()
    {
        while (true)
        {
            if (HasAxe)
                yield return base.Process();
                
            yield return null;
        }
    }

    public override void SetBool(string name)
    {
        if (name == "HasAxe")
        {  
            foreach (var i in controller.data.animators)
                i.ResetTrigger("Attack");

            controller.data.animators[0].SetBool("Axe", true);

            HasAxe = true;
        }
    }
}
