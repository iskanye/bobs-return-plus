using System.Collections;

[UnityEngine.CreateAssetMenu(fileName = "AttackTest", menuName = "Bobs/AttackTest", order = 0)]
public class TestAttackBob : BaseBob
{
    public Melee melee;

    public override IEnumerator Start()
    {
        melee.bob = this;
        yield return melee.Start();
        yield return base.Start();
    }

    public override IEnumerator Process()
    {
        while (true)
        {
            yield return melee.Process();
            yield return base.Process();
        }
    }
}
