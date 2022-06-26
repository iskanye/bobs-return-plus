using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Melee", menuName = "Skill/Melee", order = 0)]
public class Melee : BaseSkill
{
    public int damage;
    public float reload;
    public float duration;
    public int range;
    public DiscardingType discarding;
    public ObjectType penetrating;
    public BoxCollider2D meleePrefab;

    bool isReloading;
    bool attackTrigger;

    public override IEnumerator Start()
    {
        InputManager.Input.Player.Attack.started += (c) => attackTrigger = true;
        isReloading = false;

        yield return base.Start();
    }

    public override IEnumerator Process()
    {
        if (isReloading || !attackTrigger || !(DialogueSystem.Active.state is Dialogues.IdleState))
        {
            attackTrigger = false;
            yield break;
        }

        var direction = bob.data.movement.Direction;
        var dir = new Vector2(direction.y != 0 ? 0 : direction.x, direction.y);

        var collider = Instantiate(meleePrefab, bob.controller.transform);
        collider.offset = new Vector2(dir.x != 0 ? (dir.x < 0 ? -.5f : .5f) : 0, dir.y != 0 ? (dir.y < 0 ? -.5f : .5f) : 0);
        collider.size = new Vector2(dir.x != 0 ? 1 : range, dir.y != 0 ? 1 : range);

        var damageable = collider.GetComponent<Damageable>();
        damageable.damage = damage;
        damageable.discarding = discarding;
        damageable.penetrating = penetrating;
        bob.data.damageable = damageable;

        bob.data.animator.Play("Attack");

        var tempDir = bob.data.movement.dir;
        var rigid = bob.data.gameObject.GetComponent<Rigidbody2D>();
        var tempDrag = rigid.drag;

        if (tempDir != Vector2.zero)
        {
            bob.data.movement.enabled = false;
            rigid.drag = 7;
            rigid.velocity = tempDir * 10;
        }

        isReloading = true;

        yield return new WaitForSeconds(duration);

        if (tempDir != Vector2.zero)
        {
            bob.data.movement.enabled = true;
            rigid.drag = tempDrag;
        }

        Destroy(collider.gameObject);

        yield return new WaitForSeconds(reload);

        isReloading = false;
        attackTrigger = false;
    }
}

public enum DiscardingType
{
    Small = 1,
    Normal = 3,
    Big = 5
}

public enum ObjectType
{
    Fragile,
    Wood,
    Strong,
    Impenetrable
}
