using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector2 force;
    public Rigidbody2D body;

    public void Add() =>
        body.AddForce(force, ForceMode2D.Impulse);

    public void Add(GameObject p) =>
        p.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
}
