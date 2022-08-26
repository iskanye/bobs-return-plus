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

            if (value <= 0)
            {
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

    void Awake()
    {
        bob = GetComponent<BobController>();
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

        yield return new WaitForSeconds(.3f);
        Time.timeScale = 1;

        yield return new WaitForSeconds(3);
        LoadScene(GetActiveScene().buildIndex);
    }

    IEnumerator Invincible()
    {
        PlayerLiveCounter.Active.isInvincible = true;
        var time = invincibleTime;

        while (time >= 0)
        {
            foreach (var i in bob.data.renderers)
                i.color = i.color == Color.white ? new Color(0, 0, 0, 0) : Color.white;

            time -= .15f;
            yield return new WaitForSeconds(.15f);
        }

        foreach (var i in bob.data.renderers)
            i.color = Color.white;

        PlayerLiveCounter.Active.isInvincible = false;
    }

    IEnumerator Hit() 
    {
        bob.data.movement.enabled = false;
        bob.data.rigidbody.velocity = hitDirection;
        yield return new WaitForSeconds(hitDuration);
        bob.data.movement.enabled = true;
    }
}
