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
    public Rigidbody2D deadRat;  
    public float defaultSpeed;
    public float maxWaitTime;
    public int maxRotateAngle;
    public Damageable hitTrigger;
    public SimpleMovement movement;
    public PlayerWarp player;
    
    List<Rat> rats = new List<Rat>();
    float rotation;
    
    void Start()
    {
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

        hitTrigger.onDamage += (i) => StartCoroutine(_Distract(1, null));   
    }

    void Update()
    {
        movement.speed = Mathf.Clamp((transform.position.y - player.player.transform.position.y) * .857f, 3.5f, 8.5f);

        for (byte i = 0; i < rats.Count(); i++) 
        {
            rats[i].obj.rotation = Quaternion.Euler(0, 0, i * (360 / rats.Count()) + rotation);
            rats[i].animator.SetFloat("X", Mathf.Cos((rats[i].obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
            rats[i].animator.SetFloat("Y", Mathf.Sin((rats[i].obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
        }
    }

    public void KillRats(int count)
    {
        for (byte i = 0; i < count; i++)
        {
            Destroy(rats[0].obj.gameObject);
            rats.RemoveAt(0);
            var angle = Random.Range(-1, 1);
            Instantiate(deadRat, ratsSpawner.position, Quaternion.identity, ratsSpawner)
                .AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) * 2));
        }

        StartCoroutine(_Distract(1, null));
    }

    public void StartRotation() =>
        StartCoroutine(Rotation());

    public void Distract(UnityEvent<GameObject> action) =>
        StartCoroutine(_Distract(5, action));

    IEnumerator _Distract(float time, UnityEvent<GameObject> action)
    {
        hitTrigger.onDamage -= (i) => StartCoroutine(_Distract(1, null));   
        movement.StopMove();
        yield return new WaitForSeconds(time);
        action?.Invoke(gameObject);
        movement.StartMove();
        hitTrigger.onDamage += (i) => StartCoroutine(_Distract(1, null));   
    }

    IEnumerator Rotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, maxWaitTime));

            var speed = (Random.Range(0, 2) * 2 - 1) * defaultSpeed;
            var startRotation = rotation;

            while (Mathf.Abs(rotation - startRotation) <= Random.Range(10, maxRotateAngle))
            {
                rotation += speed;
                yield return null;
            }
        }
    }
}
