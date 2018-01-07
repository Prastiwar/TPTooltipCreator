using UnityEngine;
using UnityEditor;

namespace TP_TooltipEditor
{
    [CustomEditor(typeof(TPTooltipGUIData))]
    public class TPTooltipGUIDataEditor : ScriptlessTooltipEditor
    {
        TPTooltipGUIData TPTooltipData;

        void OnEnable()
        {
            TPTooltipData = (TPTooltipGUIData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("GUI Skin");
            TPTooltipData.GUISkin =
                (EditorGUILayout.ObjectField(TPTooltipData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Tooltip Data file - Path");
            EditorGUILayout.BeginHorizontal();
            TPTooltipData.TooltipDataPath = EditorGUILayout.TextField(TPTooltipData.TooltipDataPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Tooltip Assets - Path");
            EditorGUILayout.BeginHorizontal();
            TPTooltipData.TooltipAssetsPath = EditorGUILayout.TextField(TPTooltipData.TooltipAssetsPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Tooltip Hierarchy Prefab");
            TPTooltipData.TooltipPrefab = (EditorGUILayout.ObjectField(TPTooltipData.TooltipPrefab, typeof(GameObject), true) as GameObject);

            if (GUI.changed)
                EditorUtility.SetDirty(TPTooltipData);

            serializedObject.ApplyModifiedProperties();
        }
    }
}