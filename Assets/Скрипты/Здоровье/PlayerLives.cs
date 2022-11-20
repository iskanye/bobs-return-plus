using UnityEngine;
using System.Collections;
using static UnityEngine.SceneManagement.SceneManager;

public class PlayerLives : LivesBase
{
    public override int Lives
    {
        get =>
            PlayerLiveCounter.Active.LivesRemaining;

        set
        {
            if (melee && melee.melee.attackTrigger)
                return;

            bob.data.movement.speed = prevSpeed;

            if (value <= 0)
            {
                CameraController.Active.StartShake(.15f, 1.5f);
                StopAllCoroutines();
                StartCoroutine(Death());
            }

            else if (PlayerLiveCounter.Active.LivesRemaining > value)
            {
                if (PlayerLiveCounter.Active.isInvincible)
                    return;

                StopAllCoroutines();
                StartCoroutine(Invincible());

                foreach (var i in bob.data.animators)
                    i.SetTrigger("Hit");

                StartCoroutine(Hit());
                CameraController.Active.StartShake(.15f, 1f);
            }

            if (livesCalculation == null)
                PlayerLiveCounter.Active.LivesRemaining = value;

            else
                PlayerLiveCounter.Active.LivesRemaining = livesCalculation.Invoke(value);
        }
    }

    public float hitDuration;
    public float invincibleTime;
    public Heart[] livesInOneHeart;

    [HideInInspector] public Animator deathScreen;

    BobController bob;
    MeleeBob melee;
    float prevSpeed;

    void Awake()
    {
        bob = GetComponent<BobController>();
        prevSpeed = bob.data.movement.speed;
        Durability = ObjectType.Wood;

        melee = bob.character is MeleeBob meleeBob ? meleeBob : null;
        PlayerLiveCounter.Active.livesInOneHeart = livesInOneHeart;
        PlayerLiveCounter.Active.Initialize();
    }

    IEnumerator Death()
    {
        FindObjectOfType<PauseController>().gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;

        foreach (var i in bob.data.animators)
            i.SetTrigger("Death");

        bob.gameObject.layer = LayerMask.GetMask(LayerMask.LayerToName(0));
        bob.data.movement.enabled = false;

        Time.timeScale = .25f;
        StartCoroutine(Invincible());

        yield return new WaitForSeconds(.4f);
        bob.enabled = false;
        deathScreen.Play("Death");
        FadeInOut.active.FadeIn(.45f);

        yield return new WaitForSeconds(.3f);
        Time.timeScale = 1;

        yield return new WaitForSeconds(5.7f);
        FadeInOut.active.FadeOut(.6f);

        yield return new WaitForSeconds(2);
        LoadScene(GetActiveScene().buildIndex);
    }

    IEnumerator Invincible()
    {
        PlayerLiveCounter.Active.isInvincible = true;

        for (var time = invincibleTime; time >= 0; time -= .15f)
        {
            foreach (var i in bob.data.renderers)
                i.color = i.color == Color.white ? new Color(0, 0, 0, 0) : Color.white;

            yield return new WaitForSeconds(.15f);
        }

        foreach (var i in bob.data.renderers)
            i.color = Color.white;

        PlayerLiveCounter.Active.isInvincible = false;
    }

    IEnumerator Hit() 
    {
        prevSpeed = bob.data.movement.speed;
        bob.data.movement.speed *= .1f;
        bob.data.rigidbody.velocity = hitDirection;
        yield return new WaitForSeconds(hitDuration);
        bob.data.movement.speed = prevSpeed;
    }
}
