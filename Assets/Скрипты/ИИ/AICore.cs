using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
public class AICore : MonoBehaviour
{
    public float searchingTime;
    public float changePointDistance;
    public float maxSpeed;

    public Vector3 velocity => rig.velocity;
    public Vector3 position => transform.position;

    [HideInInspector] public bool reachedEndOfPath;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public bool canSearch;
    [HideInInspector] public bool canMove;

    public bool hasPath => currentPath != null;
    public bool pathPending => !seeker.IsDone();

    public Action onSearchPath;

    Rigidbody2D rig;
    Path currentPath;
    Seeker seeker;
    int index;

    void Awake() 
    {
        rig = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>(); 

        canSearch = true;
    }

    void FixedUpdate() 
    {
        if (!hasPath || !canMove || reachedEndOfPath)
        {
            if (reachedEndOfPath)
                currentPath = null;

            return;
        }

        var dir = (currentPath.vectorPath[index] - position).normalized;
        rig.velocity = dir * maxSpeed;

        if ((currentPath.vectorPath[index] - position).sqrMagnitude < changePointDistance * changePointDistance)
            index++;

        if (index >= currentPath.vectorPath.Count)
        {
            reachedEndOfPath = true;
            rig.velocity = Vector2.zero;
        }
    }

    void OnDisable()
    {
        rig.velocity = Vector2.zero;
        currentPath = null;
    }

    public void SearchPath()
    {
        if (!canSearch || pathPending)
            return;

        seeker.StartPath(position, destination, OnPathComplete);
        StartCoroutine(SearchBreak());
    }

    public void SetPath(Path path) =>
        currentPath = path;

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            onSearchPath?.Invoke();

            index = 0;
            reachedEndOfPath = false;
        }
    }

    System.Collections.IEnumerator SearchBreak() 
    {
        canSearch = false;
        yield return new WaitForSeconds(searchingTime);
        canSearch = true;
    }
}
