using UnityEditor;
using UnityEngine;

namespace TP_TooltipEditor
{
    public class ScriptlessTooltipEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (GUILayout.Button("Open Tooltip Manager", GUILayout.Height(30)))
            {
                TPTooltipDesigner.OpenWindow();
            }
        }
    }
}