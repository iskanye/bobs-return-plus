using UnityEngine;

public class AINoiseDistractor : MonoBehaviour
{
    public float range;
    public LayerMask AIMask;

    public void Distract()
    {
        var AIs = Physics2D.OverlapCircleAll(transform.position, range, AIMask);
        foreach (var i in AIs) i.GetComponent<AI>().NoiseDistraction(transform.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
