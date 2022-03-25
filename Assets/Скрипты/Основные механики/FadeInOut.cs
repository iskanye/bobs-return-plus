using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : SequenceObject
{
    [HideInInspector] public Image img;
    [HideInInspector] public bool isIn;
    [HideInInspector] public float speed;

    FadeState fadeState;
    State<FadeInOut> state;

    void Awake()
    {
        img = GetComponent<Image>();
        fadeState = new FadeState(this);
    }

    public override IEnumerator Sequence()
    {
        yield return new WaitUntil(() => state is null);
    }

    public void ChangeState(State<FadeInOut> state)
    {
        if (this.state != null)
            StartCoroutine(this.state.Stop());

        this.state = state;

        if (this.state != null)
            StartCoroutine(this.state.Start());
    }

    public void FadeOut(float speed) 
    {
        this.speed = speed;
        isIn = false;
        ChangeState(fadeState);
    }

    public void FadeIn(float speed)
    {
        this.speed = speed;
        isIn = true;
        ChangeState(fadeState);
    }
}

public class FadeState : State<FadeInOut>   
{
    public FadeState(FadeInOut mn) : base(mn) { }

    public override IEnumerator Start()
    {
        float t = 0;

        while (mn.isIn ? mn.img.color.a < 1 : mn.img.color.a > 0) 
        {
            mn.img.color = new Color(mn.img.color.r, mn.img.color.g, mn.img.color.b, Mathf.Lerp(mn.isIn ? 0 : 1, mn.isIn ? 1 : 0, t));
            t += Time.deltaTime * mn.speed;

            yield return base.Update(); 
        }

        mn.ChangeState(null);
        yield return base.Update();
    }
}
