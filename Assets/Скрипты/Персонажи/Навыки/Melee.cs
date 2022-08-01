using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Melee", menuName = "Skill/Melee", order = 0)]
public class Melee : BaseSkill
{
    public bool dash;
    public int damage;
    public float swingDuration;
    public float dashDuration;
    public float hitDuration;
    public float reloadDuration;
    public Vector2 colliderSize;
    public DiscardingType discarding;
    public ObjectType penetrating;
    public BoxCollider2D meleePrefab;

    [HideInInspector] public bool attackTrigger;

    bool isReloading;

    public override IEnumerator Start()
    {
        InputManager.Input.Player.Attack.started += (c) => attackTrigger = true;
        isReloading = false;

        yield return base.Start();
    }

    public override IEnumerator Process()
    {
        if (isReloading || !attackTrigger || !(DialogueSystem.Active.state is Dialogues.IdleState) || PlayerLiveCounter.Active.isInvincible)
        {
            attackTrigger = false;
            yield break;
        }

        var direction = bob.data.movement.Direction;
        var dir = new Vector2(direction.y != 0 ? 0 : direction.x, direction.y);
        var rigid = bob.data.gameObject.GetComponent<Rigidbody2D>();

        foreach (var i in bob.data.animators)
            i.SetTrigger("Attack");

        isReloading = true;

        var isWalk = bob.data.movement.IsWalking;
        bob.data.movement.enabled = false;

        if (isWalk && dash)
        {
            rigid.velocity += dir * 8;
            yield return new WaitForSeconds(dashDuration);
        }

        else 
            yield return new WaitForSeconds(swingDuration);

        rigid.velocity += dir * 6;

        var collider = Instantiate(meleePrefab, bob.controller.transform);
        var offset = colliderSize.x / 2;

        collider.offset = new Vector2(dir.x != 0 ? (dir.x < 0 ? -offset : offset) : 0, dir.y != 0 ? (dir.y < 0 ? -offset : offset) : 0);
        collider.size = new Vector2(dir.x != 0 ? colliderSize.x : colliderSize.y, dir.y != 0 ? colliderSize.x : colliderSize.y);

        var damageable = collider.GetComponent<Damageable>();
        damageable.damage = damage;
        damageable.discarding = discarding;
        damageable.penetrating = penetrating;
        damageable.direction = dir;
        bob.data.damageable = damageable;

        yield return new WaitForSeconds(hitDuration);

        Destroy(collider.gameObject);
        bob.data.movement.enabled = true;

        yield return new WaitForSeconds(reloadDuration);

        isReloading = false;
        attackTrigger = false;
    }

    public override void Stop()
    {
        attackTrigger = false;
        isReloading = false;
    }
}

public enum DiscardingType
{
    Small = 5,
    Normal = 8,
    Big = 11
}

public enum ObjectType
{
    Fragile,
    Wood,
    Strong,
    Impenetrable
}
