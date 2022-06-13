using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public TopDownMovement movement;
    public LivesBase lives;
    public Damageable damageable;
    public Animator animator;
    public InventorySystem inventory;
    public GameObject gameObject;
}
