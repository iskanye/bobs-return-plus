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
        var ach = achievement;
        var id = new IdItem(ach.id);

        if (!SceneData.Data.achievements.Contains(id))
        {
            title.text = ach.title;
            description.text = ach.description;
            icon.sprite = ach.icon;

            StartCoroutine(PlayAnimation());
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
