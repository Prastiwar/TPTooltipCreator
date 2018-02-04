using TP.Tooltip;
using UnityEditor;

namespace TP.TooltipEditor
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