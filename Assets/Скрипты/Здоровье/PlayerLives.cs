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
                CameraController.Active.StartShake(.1f, 1f);
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
        Durability = ObjectType.Flimsy;

        melee = bob.character is MeleeBob meleeBob ? meleeBob : null;
        PlayerLiveCounter.Active.livesInOneHeart = livesInOneHeart;
        PlayerLiveCounter.Active.Initialize();
    }

    IEnumerator Death()
    {
        StartCoroutine(SkipDeath());
        FindObjectOfType<PauseController>().gameObject.SetActive(false);

        foreach (var i in bob.data.animators)
            i.SetTrigger("Death");

        bob.gameObject.layer = LayerMask.GetMask(LayerMask.LayerToName(0));
        bob.data.movement.Disable();

        Time.timeScale = .25f;
        StartCoroutine(Invincible());

        yield return new WaitForSeconds(.4f);
        bob.enabled = false;
        deathScreen.Play("Death");

        yield return new WaitForSeconds(.3f);
        deathScreen.GetComponent<SimpleUnscaledSpriteAnimation>().StartAnimation();
        Time.timeScale = 1;

        yield return new WaitForSeconds(6.2f);
        deathScreen.GetComponent<SimpleUnscaledSpriteAnimation>().Stop();

        yield return new WaitForSeconds(4.2f);

        LoadScene(GetActiveScene().buildIndex);
    }

    IEnumerator SkipDeath() 
    {
        while (true)
        {
            if (InputManager.Active.attack)
                LoadScene(GetActiveScene().buildIndex);

            yield return null;
        }
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
        bob.data.movement.Disable();
        bob.data.rigidbody.AddForce(hitDirection, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hitDuration);  

        if (DialogueSystem.Active.state is Dialogues.IdleState)  
            bob.data.movement.Enable();
    }
}
