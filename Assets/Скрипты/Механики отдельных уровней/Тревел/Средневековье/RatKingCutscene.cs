using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKingCutscene : MonoBehaviour
{
    public FollowMovement ratPrefab;
    public Transform ratSpawner;
    public float maxStopRadius;
    public float spawnDelay;
    public float leftSpawnBorder;
    public float rightSpawnBorder;
    public float ySpawnOffset;
    public int ratLimit;
    public RatKing ratKing;
    public Transform player;
    public CameraController cam;

    List<GameObject> rats = new List<GameObject>();

    public void StartRatsSpawn() =>
        StartCoroutine(SpawnRats());

    public void StartRatKingSpawn() =>
        StartCoroutine(SpawnRatKing());

    IEnumerator SpawnRats()
    {
        var rat = Instantiate(ratPrefab, new Vector2(Random.Range(leftSpawnBorder, rightSpawnBorder), player.position.y - ySpawnOffset), Quaternion.identity, ratSpawner);
        rat.target = player;
        rat.stopRadius = Random.Range(2, maxStopRadius);
        rats.Add(rat.gameObject);
        
        if (rats.Count >= ratLimit)
        {
            Destroy(rats[0]);            
            rats.RemoveAt(0);
        }

        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnRats());
    }

    IEnumerator SpawnRatKing()
    {     
        ratKing.gameObject.SetActive(true);
        cam.StartShake(.1f, 3);

        var playerMov = player.GetComponent<TopDownMovement>();
        playerMov.Disable();

        yield return new WaitForSeconds(.5f);
        playerMov.Direction = Vector2.left;

        yield return new WaitForSeconds(.5f);
        playerMov.Direction = Vector2.down;

        yield return new WaitForSeconds(2);  

        cam.ChangeTarget(ratKing.transform);
        ratKing.StartRotation();
        ratKing.GetComponent<SimpleMovement>().StartMove();

        yield return new WaitForSeconds(1.5f);    
        
        cam.ChangeTarget(player);
        playerMov.Enable();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(leftSpawnBorder, player.position.y), .125f);
        Gizmos.DrawWireSphere(new Vector2(rightSpawnBorder, player.position.y), .125f);
        Gizmos.DrawWireSphere(new Vector2(player.position.x, player.position.y - ySpawnOffset), .125f);
    }
}
