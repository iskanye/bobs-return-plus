public class AchievementActivator : UnityEngine.MonoBehaviour
{
    public Achievement achievement;

    public void Activate() => AchievementSystem.Active.ShowAchievement(achievement);
}
