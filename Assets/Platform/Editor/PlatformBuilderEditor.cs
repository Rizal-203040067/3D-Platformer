using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformBuilder))]
public class PlatformBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformBuilder builder = (PlatformBuilder)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Build Platform"))
        {
            builder.Build();
        }

        if (GUILayout.Button("Clear Platform"))
        {
            builder.Clear();
        }

    }
}
