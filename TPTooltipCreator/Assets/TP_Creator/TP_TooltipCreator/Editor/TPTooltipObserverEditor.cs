using TP_Tooltip;
using UnityEditor;

namespace TP_TooltipEditor
{ 
    [CustomEditor(typeof(TPTooltipObserver))]
    public class TPTooltipObserverEditor : ScriptlessTooltipEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Tooltip Observer");
        }
    }
}