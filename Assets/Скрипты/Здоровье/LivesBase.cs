public class LivesBase : UnityEngine.MonoBehaviour
{
    public virtual int Lives { get; set; }
    public System.Func<int, int> livesCalculation;
}
