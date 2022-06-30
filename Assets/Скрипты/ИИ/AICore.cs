using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Seeker))]
public class AICore : MonoBehaviour, IAstarAI
{
    public float searchingTime;
    public float changePointDistance;

    public float radius { get; set; }
    public float height { get; set; }

    public Vector3 position => transform.position;

    public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }
    public float maxSpeed { get; set; }

    public Vector3 velocity => rig.velocity;

    public Vector3 desiredVelocity => throw new NotImplementedException();

    public float remainingDistance => throw new NotImplementedException();

    public bool reachedDestination => throw new NotImplementedException();

    public bool reachedEndOfPath { get; set; }

    public Vector3 destination { get; set; }
    public bool canSearch { get; set; }
    public bool canMove { get; set; }

    public bool hasPath => currentPath != null;

    public bool pathPending => !seeker.IsDone();

    public bool isStopped { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Vector3 steeringTarget => throw new NotImplementedException();

    public Action onSearchPath { get; set; }

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

        reachedEndOfPath |= index >= currentPath.vectorPath.Count;
    }

    public void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
    {
        throw new NotImplementedException();
    }

    public void GetRemainingPath(List<Vector3> buffer, out bool stale)
    {
        throw new NotImplementedException();
    }

    public void Move(Vector3 deltaPosition)
    {
        throw new NotImplementedException();
    }

    public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
    {
        throw new NotImplementedException();
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

    public void Teleport(Vector3 newPosition, bool clearPath = true)
    {
        throw new NotImplementedException();
    }

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
