public class PlayerWarp : SequenceObject
{
    public bool Enabled 
    { 
        set => 
            player.enabled = value;

        get =>
            player.enabled;
    }

    public UnityEngine.UI.Image[] effectsImages;

    [UnityEngine.HideInInspector] public BobController player;

    void Awake()
    {
        player = FindObjectOfType<BobController>();
        player.GetComponent<PlayerEffectsController>().images = effectsImages;
    }

    void Update() =>
        transform.position = player.transform.position;

    public override System.Collections.IEnumerator Sequence() 
    {
        yield return player.GetComponent<AnimationMovementController>().Sequence();
    }
}

