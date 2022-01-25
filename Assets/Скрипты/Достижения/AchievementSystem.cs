using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public UnityEngine.UI.Image icon;

    public static AchievementSystem Active { get; private set; }

    Animator animator;
    SceneData data;

    void Start()
    {
        data = SceneData.Active;
        Active = this;
        animator = GetComponent<Animator>();
    }

    public void ShowAchievement(Achievement achievement)
    {
        var ach = achievement;
        var id = new IdItem(ach.id);

        if (!data.data.achievements.Contains(id))
        {
            title.text = ach.title;
            description.text = ach.description;
            icon.sprite = ach.icon;
            StartCoroutine(PlayAnimation());
            data.data.achievements.Add(id);
        }
    }

    System.Collections.IEnumerator PlayAnimation()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(10f);
        animator.Play("Close");
    }
}
