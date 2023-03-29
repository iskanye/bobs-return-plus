using UnityEngine;
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
    public List<Rat> rats;
    public List<GameObject> ratsPrefabs;
    public GameObject albinoRat;    
    public float defaultSpeed;
    public float maxWaitTime;
    public int maxTurnoverAmmount;
    public Damageable hitTrigger;
    public SimpleMovement movement;
    
    void Start()
    {
        if (Random.value <= .2f)
            ratsPrefabs.Add(albinoRat);

        var _rats = (from i in ratsPrefabs orderby Random.value select i).ToArray();

        for (byte i = 0; i < _rats.Count(); i++)
        {
            var rat = Instantiate(_rats[i], ratsSpawner.position, Quaternion.Euler(0, 0, i * (360 / _rats.Count())), ratsSpawner);
            rats.Add(new Rat() 
            { 
                obj = rat.transform, 
                animator = rat.GetComponentInChildren<Animator>() 
            });
        }

        foreach (var i in rats) 
        {
            i.animator.SetFloat("X", Mathf.Cos((i.obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
            i.animator.SetFloat("Y", Mathf.Sin((i.obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
        }  

        hitTrigger.onDamage += (i) => StartCoroutine(PlayerHit());   
    }

    public void StartRotation() =>
        StartCoroutine(Rotation());

    IEnumerator PlayerHit()
    {
        movement.StopMove();
        yield return new WaitForSeconds(1);
        movement.StartMove();
    }

    IEnumerator Rotation()
    {
        yield return new WaitForSeconds(Random.Range(0, maxWaitTime));

        var speed = (Random.Range(0, 2) * 2 - 1) * defaultSpeed;
        var turnovers = 0;
        var currentRotation = .0f;

        while (turnovers < Random.Range(1, maxTurnoverAmmount))
        {
            currentRotation += speed;

            if (Mathf.Abs(currentRotation) >= 360)
            {
                turnovers++;
                currentRotation = 0;
            }

            foreach (var i in rats) 
            {
                i.animator.SetFloat("X", Mathf.Cos((i.obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
                i.animator.SetFloat("Y", Mathf.Sin((i.obj.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
                i.obj.eulerAngles += new Vector3(0, 0, speed);
            }

            yield return null;
        }

        StartCoroutine(Rotation());
    }
}
