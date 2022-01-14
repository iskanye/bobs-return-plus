using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI))]
public class AIEditor : Editor
{
    void OnSceneGUI()
    {
        var ai = (AI)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(ai.transform.position, Vector3.back, Vector3.down, 360, ai.seeRange);
        
        var one = dir(Vector2.Angle(Vector2.up, ai.direction), ai.viewAngle * .5f);   
        var two = dir(Vector2.Angle(Vector2.up, ai.direction), -ai.viewAngle * .5f); 
        
        Handles.color = ai.isSee ? Color.green : Color.red;  
        Handles.DrawLine(ai.transform.position, (Vector2)ai.transform.position + one * ai.seeRange);
        Handles.DrawLine(ai.transform.position, (Vector2)ai.transform.position + two * ai.seeRange);
        
        if (ai.isSee) Handles.DrawLine(ai.transform.position, ai.target);

        if (ai.isPatrol) 
        {
            Handles.color = Color.yellow;
            
            foreach (var i in ai.path) Handles.DrawWireDisc(i, Vector3.back, .1f);
        }        
    }
    Vector2 dir(float angle, float angle2) => new Vector2(Mathf.Sin((angle + angle2) * Mathf.Deg2Rad), 
    Mathf.Cos((angle + angle2) * Mathf.Deg2Rad));
}
