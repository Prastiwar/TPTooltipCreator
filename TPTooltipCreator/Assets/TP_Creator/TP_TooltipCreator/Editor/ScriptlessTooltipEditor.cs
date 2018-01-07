using UnityEditor;

namespace TP_TooltipEditor
{
    public class ScriptlessTooltipEditor : Editor
    {
        public readonly string[] scriptField = new string[] { "m_Script" };

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);
        }
    }
}