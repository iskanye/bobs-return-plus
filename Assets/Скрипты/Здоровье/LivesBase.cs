public class LivesBase : UnityEngine.MonoBehaviour
{
    public virtual int Lives { get; set; }
    public delegate int LivesCalculation(int lives);
    public LivesCalculation livesCalculation;
}
