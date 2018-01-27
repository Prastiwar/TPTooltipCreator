using TP_Tooltip;
using UnityEditor;

namespace TP_TooltipEditor
{
    [CustomEditor(typeof(TPTooltipCreator))]
    internal class TPTooltipCreatorEditor : ScriptlessTooltipEditor
    {
        string OBJObservers = "OBJObservers";

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("This script allows you to manage your Tooltip");

            if (TPTooltipCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField, OBJObservers);

            OpenCreator();
        }
    }
}