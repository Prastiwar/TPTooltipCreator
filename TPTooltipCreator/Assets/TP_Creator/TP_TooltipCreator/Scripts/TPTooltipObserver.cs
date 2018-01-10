﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace TP_TooltipCreator
{
    public class TPTooltipObserver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum ToolTipType
        {
            DynamicEnter,
            DynamicClick,
            StaticEnter,
            StaticClick
        }
        public ToolTipType SetType;
        TPTooltipCreator tooltipCreator;

        void OnValidate()
        {
            Awake();
        }

        void Awake()
        {
            if (tooltipCreator == null) tooltipCreator = FindObjectOfType<TPTooltipCreator>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (SetType == ToolTipType.DynamicClick || SetType == ToolTipType.StaticClick)
                tooltipCreator.OnPointerEnter(eventData);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SetType == ToolTipType.DynamicEnter || SetType == ToolTipType.StaticEnter)
                tooltipCreator.OnPointerEnter(eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(SetType != ToolTipType.StaticClick)
                tooltipCreator.OnPointerExit(eventData);
        }
    }
}