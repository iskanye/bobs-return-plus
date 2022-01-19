using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapObject : MonoBehaviour
{
    private LiveCounter player;

    private void Start()
    {
        player = FindObjectOfType<LiveCounter>();
    }

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // преп€тствие снимает жизьки сразу, без периодов
            player.LoseLife();
        }
    }
}
