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

    [HideInInspector] public bool attackTrigger => 
        !isReloading && InputManager.Active.attack && !PlayerLiveCounter.Active.isInvincible;

    bool isReloading;

    public override IEnumerator Process()
    {
        if (!attackTrigger)        
            yield break;

        isReloading = true;

        var direction = bob.data.movement.Direction;
        var dir = new Vector2(direction.y > 0.1f && direction.y < -0.1f ? 0 : direction.x, direction.y);
        var rigid = bob.data.rigidbody;

        foreach (var i in bob.data.animators)
            i.SetTrigger("Attack");

        var isWalk = bob.data.movement.IsWalking;
        bob.data.movement.Disable();

        if (isWalk && dash)
        {
            rigid.velocity = dir * 2;
            yield return new WaitForSeconds(dashDuration);
        }

        else 
            yield return new WaitForSeconds(swingDuration);

        var collider = Instantiate(meleePrefab, bob.controller.transform);
        var offset = colliderSize.x / 2;

        collider.offset = new Vector2(colliderSize.x * .5f, 0);
        collider.size = colliderSize;
        collider.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));

        var damageable = collider.GetComponent<Damageable>();
        damageable.damage = damage;
        damageable.discarding = discarding;
        damageable.penetrating = penetrating;
        damageable.direction = dir;
        bob.data.damageable = damageable;

        var inTime = collider.GetComponent<ActionInTime>();
        inTime.time = .1f;
        inTime.Action();

        yield return new WaitForSeconds(hitDuration);

        bob.data.movement.Enable();

        yield return new WaitForSeconds(reloadDuration);

        isReloading = false; 
    }

    public override void Stop()
    {
        isReloading = false;
    }
}

public enum DiscardingType
{
    Small = 1,
    Normal = 8,
    Big = 15
}

public enum ObjectType
{
    Fragile,
    Flimsy,
    Wood,
    Strong,
    Impenetrable
}
