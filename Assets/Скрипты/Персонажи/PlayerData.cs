using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public SpriteRenderer[] renderers;
    public TopDownMovement movement;
    public PlayerLives lives;
    public Damageable damageable;
    public Animator[] animators;
    public InventorySystem inventory;
    public GameObject gameObject;
}
