using TP_Tooltip;
using UnityEditor;

namespace TP_TooltipEditor
{ 
    [CustomEditor(typeof(TPTooltipObserver))]
    internal class TPTooltipObserverEditor : ScriptlessTooltipEditor
    {
        void OnEnable()
        {
            if (serializedObject.targetObject.hideFlags != UnityEngine.HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = UnityEngine.HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Tooltip Observer");
            if (TPTooltipCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);
        }
    }
}