using UnityEngine;
using System.Collections;

public class TutorialZombie : MonoBehaviour
{
    public int damage;
    public DiscardingType discarding;
    public GameObject zombieHead;
    public Vector2 colliderSize;
    public BoxCollider2D meleePrefab;
    public float swingDuration;
    public float hitDuration;
    public float reloadDuration;
    public float invincibleTime;

    AIManager ai;
    SpriteRenderer rend;
    Animator anim;
    Rigidbody2D rigid;
    LivesBase lives;

    bool isInvincible;
    bool isReloading;

    void Awake() 
    {
        rend = GetComponent<SpriteRenderer>();
        ai = GetComponent<AIManager>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        lives = GetComponent<LivesBase>();
    }

    public void OnHit()
    {
        if (isInvincible)
            return;

        StopAllCoroutines();
        isReloading = false;
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        ai.enabled = false;
        yield return _Hit();
        ai.enabled = true;

        ai.ChangeState(ai.searchState);
    }

    IEnumerator _Hit() 
    {
        isInvincible = true;
        rigid.velocity = lives.hitDirection;

        var time = invincibleTime;

        while (time >= 0)
        {
            rend.color = rend.color == Color.white ? new Color(1, .5f, .5f, 1) : Color.white;

            time -= .15f;
            yield return new WaitForSeconds(.15f);
        }

        rend.color = Color.white;
        isInvincible = false;
    }

    public void Attack()
    {
        if (isReloading || isInvincible)
            return;

        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine() 
    {
        anim.SetTrigger("Attack");

        isReloading = true;
        ai.enabled = false;

        yield return new WaitForSeconds(swingDuration);

        var coll = Instantiate(meleePrefab, transform);

        coll.offset = new Vector2(colliderSize.x * .5f, 0);
        coll.size = colliderSize;
        coll.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, ai.Direction.normalized));

        var damageable = coll.GetComponent<Damageable>();
        damageable.damage = damage;
        damageable.discarding = discarding;
        damageable.penetrating = ObjectType.Fragile;
        damageable.direction = ai.Direction;

        var inTime = coll.GetComponent<ActionInTime>();
        inTime.time = hitDuration;
        inTime.Action();

        yield return new WaitForSeconds(reloadDuration + hitDuration);

        isReloading = false;
        ai.enabled = true;
    }

    public void OnDeath()
    {
        lives.StartCoroutine(_Hit());

        GetComponent<Collider2D>().enabled = false;

        var head = Instantiate(zombieHead, transform.position, Quaternion.identity);
        var rig = head.GetComponent<Rigidbody2D>();
        rig.velocity = lives.hitDirection.normalized * 10;

        anim.SetTrigger("Death");

        Destroy(ai.AI);
        Destroy(ai);
        Destroy(this);
    }
}
