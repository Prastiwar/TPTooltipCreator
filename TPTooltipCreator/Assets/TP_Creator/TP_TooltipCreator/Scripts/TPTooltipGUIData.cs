using UnityEngine;

namespace TP.TooltipEditor
{
    public class TPTooltipGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public GameObject TooltipPrefab;
    }
}