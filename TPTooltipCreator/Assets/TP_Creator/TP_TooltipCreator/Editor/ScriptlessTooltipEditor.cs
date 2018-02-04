using TP.Tooltip;
using UnityEditor;
using UnityEngine;

namespace TP.TooltipEditor
{
    internal class ScriptlessTooltipEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (TPTooltipCreator.DebugMode)
            {
                if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                    serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
                return;
            }

            if (serializedObject.targetObject.hideFlags != HideFlags.None)
                serializedObject.targetObject.hideFlags = HideFlags.None;

            if (GUILayout.Button("Open Tooltip Manager", GUILayout.Height(30)))
            {
                TPTooltipDesigner.OpenWindow();
            }
        }
    }
}