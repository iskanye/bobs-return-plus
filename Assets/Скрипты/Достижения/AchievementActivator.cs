using UnityEngine;

public class AchievementActivator : MonoBehaviour
{
    public Achievement achievement;
    public AchievementSystem system;

    public void Activate() => system.ShowAchievement(achievement);
}
