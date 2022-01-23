using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IWalkable 
{
    bool IsWalking {get;}
    Vector2 Direction {get;}
}
