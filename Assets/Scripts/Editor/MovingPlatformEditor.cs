using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private MovingPlatform movingPlatform;
    
    void OnEnable()
    {
        this.movingPlatform = this.target as MovingPlatform;    
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        this.movingPlatform.controlledByLever = EditorGUILayout.Toggle("Controlled by Lever", this.movingPlatform.controlledByLever);
    }

    void OnSceneGUI()
    {
        Handles.color = Color.red;

        Handles.PositionHandle(this.movingPlatform.waypoints.Last().position, Quaternion.identity);
    }
}
