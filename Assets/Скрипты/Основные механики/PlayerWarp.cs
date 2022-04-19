public class PlayerWarp : SequenceObject
{
    public bool Enabled { set => player.enabled = value; }

    TopDownMovement player;

    void Awake() =>
        player = FindObjectOfType<TopDownMovement>();

    void Update() =>
        transform.position = player.transform.position;

    public override System.Collections.IEnumerator Sequence() 
    {
        yield return player.GetComponent<AnimationMovementController>().Sequence();
    }
}

