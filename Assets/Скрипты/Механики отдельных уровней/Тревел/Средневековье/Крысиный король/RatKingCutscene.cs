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
    public PlayerWarp player;
    public CameraController cam;

    Coroutine spawnRats;

    public void StartRatsSpawn() =>
        spawnRats = StartCoroutine(SpawnRats());

    public void StartRatKingSpawn() =>
        StartCoroutine(SpawnRatKing());

    IEnumerator SpawnRats()
    {
        while (true)
        {
            var rat = Instantiate(ratPrefab, new Vector2(Random.Range(leftSpawnBorder, rightSpawnBorder), 
                player.player.transform.position.y - ySpawnOffset), Quaternion.identity, ratSpawner);
            rat.target = player.player.transform;
            rat.stopRadius = Random.Range(2, maxStopRadius);
        
            if (ratSpawner.childCount >= ratLimit)
                Destroy(ratSpawner.GetChild(0));  

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnRatKing()
    {     
        StopCoroutine(spawnRats);
        ratKing.gameObject.SetActive(true);
        cam.StartShake(.1f, 3);
        player.Enabled = false;

        var playerMov = player.player.data.movement;

        yield return new WaitForSeconds(.5f);
        playerMov.Direction = Vector2.left;

        yield return new WaitForSeconds(.5f);
        playerMov.Direction = Vector2.down;

        yield return new WaitForSeconds(2);  

        cam.ChangeTarget(ratKing.transform);
        ratKing.StartRotation();
        ratKing.GetComponent<SimpleMovement>().StartMove();

        yield return new WaitForSeconds(1.5f);    
        
        cam.ChangeTarget(player.player.transform);
        player.Enabled = true;
    }
}
