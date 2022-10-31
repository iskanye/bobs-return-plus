using UnityEngine;

public class SceneTimer : SequenceObject
{
    bool timerTrigger;
    float waitTime;

    public void StartTimer(float time)
    {
        timerTrigger = false;
        waitTime = Time.time + time;
    }

    void Update() 
    {
        timerTrigger |= waitTime <= Time.time;
    }

    public override System.Collections.IEnumerator Sequence()
    {
        yield return new WaitUntil(() => timerTrigger);
    }
}

