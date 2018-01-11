using UnityEngine;

namespace TP_TooltipEditor
{
    public class TPTooltipGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public GameObject TooltipPrefab;
    }
}