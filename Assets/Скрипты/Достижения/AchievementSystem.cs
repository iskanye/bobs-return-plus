using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public UnityEngine.UI.Image icon;

    Animator animator;

    void Awake() =>
        animator = GetComponent<Animator>();

    public void ShowAchievement(Achievement achievement)
    {
        var id = new GuidItem(achievement.id);
        
        if (SceneData.Data.achievements.Contains(id))
            return;

        title.text = achievement.title;
        description.text = achievement.description;
        icon.sprite = achievement.icon;

        StartCoroutine(PlayAnimation());
        SceneData.Data.achievements.Add(id);
    }

    System.Collections.IEnumerator PlayAnimation()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(2);
        animator.Play("Close");
    }
}
