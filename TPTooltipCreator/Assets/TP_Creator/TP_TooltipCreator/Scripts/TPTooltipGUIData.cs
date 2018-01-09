using UnityEngine;

namespace TP_TooltipEditor
{
    public class TPTooltipGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        //[HideInInspector] public string TooltipDataPath;
        //[HideInInspector] public string TooltipAssetsPath;
        [HideInInspector] public GameObject TooltipPrefab;
    }
}