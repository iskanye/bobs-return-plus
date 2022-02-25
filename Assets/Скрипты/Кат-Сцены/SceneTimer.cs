using UnityEngine;

public class SceneTimer : SequenceObject
{
    bool timerTrigger;
    float waitTime = float.PositiveInfinity;

    public void StartTimer(float time) =>
        waitTime = Time.time + time;

    void Update() 
    {
        if (waitTime <= Time.time)
        {
            timerTrigger = true;
            waitTime = float.PositiveInfinity;
        }
    }

    public override System.Collections.IEnumerator Sequence()
    {
        yield return new WaitUntil(() => timerTrigger);

        timerTrigger = false;
    }
}

