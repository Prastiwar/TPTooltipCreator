using TP_TooltipCreator;
using UnityEditor;

namespace TP_TooltipEditor
{ 
    [CustomEditor(typeof(TPTooltipObserver))]
    public class TPTooltipObserverEditor : ScriptlessTooltipEditor
    {
        string SetType = "SetType";

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Tooltip Observer");

            DrawPropertiesExcluding(serializedObject, scriptField, SetType);
        }
    }
}