using UnityEngine;
using System.Collections;

namespace CameraStates
{
    public class FollowState : State<CameraController>
    {
        public FollowState(CameraController manager) : base(manager) {}

        public override IEnumerator Update()
        {
            while (true)
            {
                mn.transform.localPosition = mn.target.position + mn.offset;
                yield return base.Update();
            }
        }
    }

    public class ChangeTargetState : State<CameraController>
    {
        public ChangeTargetState(CameraController manager) : base(manager) {}

        public override IEnumerator Update()
        {
            var startPos = mn.transform.localPosition;
            float t = 0;

            while (t <= 1)
            {
                mn.transform.localPosition = Vector3.Lerp(startPos, mn.target.position + mn.offset, t);
                t += Time.deltaTime * mn.cameraSpeed;
                yield return base.Update();
            }

            mn.ChangeState(mn.followState);
        }
    }
}