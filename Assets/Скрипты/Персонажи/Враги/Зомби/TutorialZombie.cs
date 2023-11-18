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

    [Header("Without leg")]

    public Rigidbody2D[] legLumps;
    public int livesWithoutLeg;
    public GameObject deathPuddle;
    public GameObject puddle;
    public float puddleSpawnDuration;
    public Vector3 headOffset; 
    public Rigidbody2D[] headLumps;

    AIManager ai;
    SpriteRenderer rend;
    Animator anim;
    Rigidbody2D rigid;
    LivesBase lives;
    BoxCollider2D poison;

    bool isInvincible;
    bool isReloading;
    bool withoutLeg;

    void Awake() 
    {
        rend = GetComponent<SpriteRenderer>();
        ai = GetComponent<AIManager>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        lives = GetComponent<LivesBase>();
    }

    void Update()
    {
        if (withoutLeg)
            poison.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, ai.Direction.normalized));
    }

    public void OnHit()
    {
        if (isInvincible)
            return;

        StopAllCoroutines();

        if (withoutLeg)
            StartCoroutine(PuddleSpawn());

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

        for (var time = invincibleTime; time >= 0; time -= .15f)
        {
            rend.color = rend.color == Color.white ? new Color(1, .5f, .5f, 1) : Color.white;
            yield return new WaitForSeconds(.15f);
        }

        rend.color = Color.white;
        isInvincible = false;
    }

    public void Attack()
    {
        if (withoutLeg || isReloading || isInvincible)
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

        var inTime = coll.GetComponent<ActionInTime>();
        inTime.time = hitDuration - .2f;
        inTime.Action();

        coll.offset = new Vector2(colliderSize.x * .5f, 0);
        coll.size = colliderSize;
        coll.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, ai.Direction.normalized));

        var damageable = coll.GetComponent<Damageable>();
        damageable.damage = damage;
        damageable.discarding = discarding;
        damageable.penetrating = ObjectType.Flimsy;
        damageable.direction = ai.Direction;

        yield return new WaitForSeconds(reloadDuration + hitDuration);

        isReloading = false;
        ai.enabled = true;
    }

    IEnumerator PuddleSpawn()
    {
        Instantiate(puddle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(puddleSpawnDuration);
        StartCoroutine(PuddleSpawn());
    }

    public void OnDeath()
    {
        lives.StartCoroutine(_Hit());

        if (!withoutLeg && Random.value < .5f) 
        {
            StartCoroutine(PuddleSpawn());

            withoutLeg = true;
            ((LivesManager)lives).Revive(livesWithoutLeg);
            anim.SetBool("Without Leg", true);

            foreach (var i in legLumps) 
                Instantiate(i, transform.position, Quaternion.identity).velocity = (Vector2)Random.onUnitSphere * 3;

            poison = Instantiate(meleePrefab, transform);
            poison.size = colliderSize;
            poison.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, ai.Direction.normalized));

            ai.enabled = true;
            ai.isPatrol = false;
            ai.ChangeState(ai.searchState);
            return;
        }

        if (withoutLeg) 
        {
            foreach (var i in headLumps)
                Instantiate(i, transform.position + headOffset, Quaternion.identity).velocity = (Vector2)Random.onUnitSphere * 3;

            Destroy(poison.gameObject);
        }

        GetComponent<Collider2D>().enabled = false;

        var head = Instantiate(withoutLeg ? deathPuddle : zombieHead, transform.position + (withoutLeg ? headOffset : Vector3.zero), Quaternion.identity);

        if (!withoutLeg)
        {
            var rig = head.GetComponent<Rigidbody2D>();
            rig.velocity = lives.hitDirection.normalized * 10;
        }

        anim.SetTrigger("Death");

        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
            
        Destroy((MonoBehaviour)ai.AI);
        Destroy(ai);
        Destroy(this);
    }
}
