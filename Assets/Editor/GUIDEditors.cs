using UnityEditor;

[CustomEditor(typeof(GUIDHolder), true)]
public class GUIDHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (UnityEngine.GUILayout.Button("Reset GUID"))
            ((GUIDHolder)target).Reset();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(GUIDScriptableObject), true)]
public class GUIDScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (UnityEngine.GUILayout.Button("Reset GUID"))
            ((GUIDScriptableObject)target).Reset();

        base.OnInspectorGUI();
    }
}

