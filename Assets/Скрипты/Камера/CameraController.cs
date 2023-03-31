using UnityEngine.U2D;
using UnityEngine;
using CameraStates;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    public Transform target;
    public Vector3 offset;
    public float cameraSpeed = 1;
    public ShakeObject shaker;

    [HideInInspector] public FollowState followState;
    [HideInInspector] public ChangeTargetState changeTargetState;

    State<CameraController> state;
    bool isShaking;

    void Awake() 
    {
        Active = this;    

        followState = new FollowState(this);
        changeTargetState = new ChangeTargetState(this);

        ChangeState(followState);

#if UNITY_ANDROID    
        Screen.SetResolution(Screen.width, Screen.height, true);
#endif
    }

    public void ChangeState(State<CameraController> state) 
    {
        if (this.state != null)
            StartCoroutine(this.state.Stop());

        this.state = state;
        StartCoroutine(this.state.Start());
    }

    public void ChangeTarget(Transform target)
    { 
        ChangeState(changeTargetState);
        this.target = target;
    }

    public void StartShake(float magnitude, float time) =>
        shaker.StartShake(magnitude, time);
}