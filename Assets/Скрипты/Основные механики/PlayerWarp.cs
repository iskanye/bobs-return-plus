public class PlayerWarp : SequenceObject
{
    public bool Enabled 
    { 
        set 
        {
            if (value)
                player.data.movement.Enable();
            else 
                player.data.movement.Disable();
        }
    }

    public UnityEngine.UI.Image[] effectsImages;
    public InventorySystem inventory;
    public UnityEngine.Animator deathScreen;

    public BobController player;

    void Awake()
    {
        player = FindObjectOfType<BobController>();
        player.GetComponent<PlayerEffectsController>().images = effectsImages;
        player.data.inventory = inventory;
        player.data.lives.deathScreen = deathScreen;
    }

    void Update() =>
        transform.position = player.transform.position;

    public override System.Collections.IEnumerator Sequence() 
    {
        yield return player.GetComponent<AnimationMovementController>().Sequence();
    }

    public void PlayAnimation(string animation) 
    {
        foreach (var i in player.data.animators)
            i.Play(animation);
    }

    public void SetCharacterBool(string name) =>
        player.SetCharacterBool(name);
}
