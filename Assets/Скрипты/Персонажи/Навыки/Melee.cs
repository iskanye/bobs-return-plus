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
    public BoxCollider2D meleePrefab;

    bool isReloading;

    public override IEnumerator Start()
    {
        yield return base.Start();
    }

    public override IEnumerator Update()
    {
        if (isReloading)
            yield break;

        var direction = bob.controller.data.movement.Direction;
        var dir = new Vector2(direction.y != 0 ? 0 : direction.x, direction.y);

        var collider = Instantiate(meleePrefab, bob.controller.transform);
        collider.offset = new Vector2(dir.x != 0 ? (dir.x < 0 ? -.5f : .5f) : 0, dir.y != 0 ? (dir.y < 0 ? -.5f : .5f) : 0);
        collider.size = new Vector2(dir.x != 0 ? 1 : range, dir.y != 0 ? 1 : range);
        collider.GetComponent<Damageable>().damage = damage;

        yield return new WaitForSeconds(duration);

        Destroy(collider.gameObject);
        isReloading = true;

        yield return new WaitForSeconds(reload);

        isReloading = false;
    }
}

public enum DiscardingType
{
    Small,
    Normal,
    Big
}
