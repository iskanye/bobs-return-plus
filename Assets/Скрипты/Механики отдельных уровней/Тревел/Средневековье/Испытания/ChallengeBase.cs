public class ChallengeBase : UnityEngine.MonoBehaviour
{
    public UnityEngine.Events.UnityEvent ifChallengeDone;

    public virtual void StartChallenge() { }

    public virtual void StopChallenge() { }
}
