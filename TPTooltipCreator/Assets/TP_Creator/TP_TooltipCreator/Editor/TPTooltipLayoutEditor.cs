using UnityEditor;
using TP_Tooltip;

namespace TP_TooltipEditor
{
    [CustomEditor(typeof(TPTooltipLayout))]
    public class TPTooltipLayoutEditor : ScriptlessTooltipEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Tooltip Layout");

            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}