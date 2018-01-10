using UnityEditor;
using TP_TooltipCreator;

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