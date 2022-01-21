using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public UnityEngine.UI.Image icon;
    Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    public void ShowAchievement(Achievement achievement) 
    {
        var ach = achievement;

        if (PlayerPrefs.HasKey(ach.id.ToString())) return;

        title.text = ach.title;
        description.text = ach.description;
        icon.sprite = ach.icon;
        StartCoroutine(PlayAnimation());
        PlayerPrefs.SetInt(ach.id.ToString(), 0);
    }

    System.Collections.IEnumerator PlayAnimation()
    {        
        animator.Play("Open");
        yield return new WaitForSeconds(10f);        
        animator.Play("Close");
    }
}
