using UnityEngine;

public class CannonLine : MonoBehaviour
{
    public Cannon cannon;
    public Transform lineParent;
    public GameObject dashedLine;
    public GameObject dashedLineEnd;
    public GameObject lineBackground;

    void Awake()
    {
        lineBackground.transform.localPosition = cannon.Offset + (Vector2)cannon.Direction * cannon.DetectionArea.x / 2 - 
            (cannon.Direction.x != 0 ? new Vector2(.5f, 0) * cannon.Direction.x : new Vector2(0, .5f) * cannon.Direction.y);
        lineBackground.transform.localScale = new Vector2(cannon.DetectionArea.x - 1, 1);
        lineBackground.transform.localRotation = Quaternion.Euler(0, 0, cannon.Direction.x != 0 ? 0 : 90);

        for (var i = 0; i < cannon.DetectionArea.x - .5f; i++)
        {
            var _dashedLine = Instantiate(dashedLine, lineParent);
            _dashedLine.transform.localPosition = (cannon.Direction.x != 0 ? new Vector3(i, 0) : new Vector3(0, i)) *
                new Vector2(cannon.Direction.x >= 0 ? 1 : -1, cannon.Direction.y >= 0 ? 1 : -1);
            _dashedLine.transform.localRotation = Quaternion.Euler(0, 0, cannon.Direction.x != 0 ? 0 : 90);            
        }

        var j = cannon.DetectionArea.x - .5f;
        var lineEnd = Instantiate(dashedLineEnd, lineParent);
        lineEnd.transform.localPosition = (cannon.Direction.x != 0 ? new Vector3(j, 0) : new Vector3(0, j)) * 
            new Vector2(cannon.Direction.x >= 0 ? 1 : -1, cannon.Direction.y >= 0 ? 1 : -1);
        lineEnd.transform.localRotation = Quaternion.Euler(0, 0, cannon.Direction.x != 0 ? 0 : (90 * cannon.Direction.y));              
        lineEnd.transform.localScale = new Vector2(cannon.Direction.x >= 0 ? 1 : -1, cannon.Direction.y >= 0 ? 1 : -1);  
    }
}
