using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector2 force;
    public Rigidbody2D body;

    public void Add() =>
        body.velocity += force;

    public void Add(GameObject p) =>
        p.GetComponent<Rigidbody2D>().velocity += force;
}
