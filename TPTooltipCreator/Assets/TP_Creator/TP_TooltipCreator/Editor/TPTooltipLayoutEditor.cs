using UnityEditor;
using TP.Tooltip;

namespace TP.TooltipEditor
{
    [CustomEditor(typeof(TPTooltipLayout))]
    internal class TPTooltipLayoutEditor : ScriptlessTooltipEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Tooltip Layout");

            if(TPTooltipCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}