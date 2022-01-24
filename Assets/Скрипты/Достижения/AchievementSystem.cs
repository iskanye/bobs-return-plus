using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public UnityEngine.UI.Image icon;

    Animator animator;
    List<AchievementJson> achievements;

    void Awake()
    {
        animator = GetComponent<Animator>();
        var ach = SaveLoad.GetAchievements();

        if (ach != null) achievements = ach.ToList();

        else achievements = new List<AchievementJson>();
    }

    public void ShowAchievement(Achievement achievement)
    {
        var ach = achievement;

        if (!achievements.Exists(a => a.id == ach.id))
        {
            title.text = ach.title;
            description.text = ach.description;
            icon.sprite = ach.icon;
            StartCoroutine(PlayAnimation());
            achievements.Add(new AchievementJson(ach.id));
            SaveLoad.SaveAchievement(achievements.ToArray());
        }
    }

    System.Collections.IEnumerator PlayAnimation()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(10f);
        animator.Play("Close");
    }
}
