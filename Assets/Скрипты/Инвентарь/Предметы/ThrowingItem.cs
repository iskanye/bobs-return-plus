using UnityEngine;

[CreateAssetMenu(fileName = "Throwing Item", menuName = "Items/Throwing Item", order = 0)]
public class ThrowingItem : Item
{
    public Rigidbody2D bullet;
    public float force;

    public override bool Action()
    {
        var bull = Instantiate(bullet, mn.warp.player.transform.position, Quaternion.identity);
        bull.AddForce(force * mn.warp.player.data.movement.Direction, ForceMode2D.Impulse);

        var anim = bull.GetComponent<Animator>();

        if (anim)
        {
            anim.SetFloat("Direction X", mn.warp.player.data.movement.Direction.x);
            anim.SetFloat("Direction Y", mn.warp.player.data.movement.Direction.y);
        }

        return true;
    }
}
