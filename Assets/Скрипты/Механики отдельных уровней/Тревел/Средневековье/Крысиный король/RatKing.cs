using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class RatKing : MonoBehaviour
{
    [System.Serializable]
    public struct Rat 
    {
        public Transform obj;
        public Animator animator;
    }
    public Transform ratsSpawner;
    public GameObject defaultRat;    
    public GameObject albinoRat;  
    public Rigidbody2D[] deadRats;  
    public float defaultSpeed;
    public float maxWaitTime;
    public Damageable hitTrigger;
    public SimpleMovement movement;
    public PlayerWarp player;
    
    List<Rat> rats = new List<Rat>();
    float rotation;
    bool hurt = false;
    Rigidbody2D rig;
    
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        for (byte i = 0; i < 6; i++)
        {
            var rat = Instantiate(defaultRat, ratsSpawner.position, Quaternion.identity, ratsSpawner);
            rats.Add(new Rat() 
            { 
                obj = rat.transform, 
                animator = rat.GetComponentInChildren<Animator>() 
            });
        }  

        if (Random.value <= .2f)
        {
            var rat = Instantiate(albinoRat, ratsSpawner.position, Quaternion.identity, ratsSpawner);
            rats.Add(new Rat() 
            { 
                obj = rat.transform, 
                animator = rat.GetComponentInChildren<Animator>() 
            });
        }

        hitTrigger.onDamage += i => StartCoroutine(_Distract(2));
    }

    void Update()
    {
        movement.speed = Mathf.Clamp((transform.position.y - player.player.transform.position.y) * .8f, 
            .8f * player.player.data.movement.speed, 1.6f * player.player.data.movement.speed);

        for (byte i = 0; i < rats.Count(); i++) 
        {
            rats[i].obj.rotation = Quaternion.Euler(0, 0, i * (360 / rats.Count()) + rotation);
            rats[i].animator.SetFloat("X", Mathf.Cos((rats[i].obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
            rats[i].animator.SetFloat("Y", Mathf.Sin((rats[i].obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
        }
    }

    public void KillRats(int count)
    {
        if (hurt)
            return;

        hurt = true;

        for (byte i = 0; i < count; i++)
        {
            Destroy(rats[0].obj.gameObject);
            rats.RemoveAt(0);
            var angle = Random.Range(-.5f, .5f) + .5f * Mathf.PI;
            Instantiate(deadRats[Random.Range(0, deadRats.Length - 1)], ratsSpawner.position, Quaternion.identity, ratsSpawner)
                .AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 10, ForceMode2D.Impulse);
        }

        rotation = 0;
        StopAllCoroutines();
        StartCoroutine(KillRats());
    }

    IEnumerator KillRats()
    {
        yield return new WaitForSeconds(maxWaitTime);

        var speed = (Random.Range(0, 2) * 2 - 1) * defaultSpeed * 5;
        var startRotation = rotation;

        while (Mathf.Abs(rotation - startRotation) <= 720)
        {
            rotation += speed;
            yield return null;
        }        
        
        rotation = 0;
        hurt = false;
        StartCoroutine(_Distract(2));
        StartCoroutine(Rotation());
    }

    public void StartRotation() =>
        StartCoroutine(Rotation());

    public void Distract(float time) =>
        StartCoroutine(_Distract(time));

    IEnumerator _Distract(float time)
    {        
        hitTrigger.onDamage -= i => StartCoroutine(_Distract(2));
        movement.StopMove();
        yield return new WaitForSeconds(time);
        movement.StartMove();
        hitTrigger.onDamage += i => StartCoroutine(_Distract(2)); 
    }

    IEnumerator Rotation()
    {
        while (rats.Count() != 1)
        {
            yield return new WaitForSeconds(Random.Range(0, maxWaitTime));

            var speed = (Random.Range(0, 2) * 2 - 1) * defaultSpeed;
            var startRotation = rotation;

            while (Mathf.Abs(rotation - startRotation) <= 360 / rats.Count())
            {
                rotation += speed;
                yield return null;
            }

            rotation = startRotation + (speed / defaultSpeed) * 360 / rats.Count();
        }
    }
}
