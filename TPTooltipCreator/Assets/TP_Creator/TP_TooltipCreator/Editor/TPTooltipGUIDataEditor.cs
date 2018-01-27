using UnityEngine;
using UnityEditor;
using TP_Tooltip;

namespace TP_TooltipEditor
{
    [CustomEditor(typeof(TPTooltipGUIData))]
    internal class TPTooltipGUIDataEditor : ScriptlessTooltipEditor
    {
        TPTooltipGUIData TPTooltipData;

        void OnEnable()
        {
            TPTooltipData = (TPTooltipGUIData)target;
            if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Container for editor data");
            if (TPTooltipCreator.DebugMode)
                return;

            EditorGUILayout.LabelField("GUI Skin");
            TPTooltipData.GUISkin =
                (EditorGUILayout.ObjectField(TPTooltipData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Tooltip Hierarchy Prefab");
            TPTooltipData.TooltipPrefab = (EditorGUILayout.ObjectField(TPTooltipData.TooltipPrefab, typeof(GameObject), true) as GameObject);
        }
    }
}