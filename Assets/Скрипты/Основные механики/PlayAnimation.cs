using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public new string animation;

    public void Play(GameObject o) =>
        o.GetComponent<Animator>().Play(animation);
}

