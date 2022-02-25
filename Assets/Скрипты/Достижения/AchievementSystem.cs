using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public UnityEngine.UI.Image icon;

    public static AchievementSystem Active { get; private set; }

    Animator animator;

    void Awake()
    {
        Active = this;

        animator = GetComponent<Animator>();
    }

    public static void ShowAchievement(Achievement achievement)
    {
        var active = Active;
        var ach = achievement;
        var id = new IdItem(ach.id);

        if (!SceneData.Data.achievements.Contains(id))
        {
            active.title.text = ach.title;
            active.description.text = ach.description;
            active.icon.sprite = ach.icon;

            active.StartCoroutine(active.PlayAnimation());
            SceneData.Data.achievements.Add(id);
        }
    }

    System.Collections.IEnumerator PlayAnimation()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(10f);
        animator.Play("Close");
    }
}
