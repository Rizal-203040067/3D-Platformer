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
            Undo.RegisterFullObjectHierarchyUndo(
                builder.gameObject,
                "Build Platform"
            );

            builder.Build();

            EditorUtility.SetDirty(builder.gameObject);
        }

        if (GUILayout.Button("Clear Platform"))
        {
            Undo.RegisterFullObjectHierarchyUndo(
                builder.gameObject,
                "Clear Platform"
            );

            builder.Clear();

            EditorUtility.SetDirty(builder.gameObject);
        }
    }
}
