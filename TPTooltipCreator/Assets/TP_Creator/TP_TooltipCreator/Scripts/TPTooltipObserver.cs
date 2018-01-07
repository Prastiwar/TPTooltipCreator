using UnityEngine;
using UnityEngine.EventSystems;

namespace TP_TooltipCreator
{
    public class TPTooltipObserver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        TPTooltipCreator tooltipCreator;

        void OnValidate()
        {
            Awake();
        }

        void Awake()
        {
            if (tooltipCreator == null) tooltipCreator = FindObjectOfType<TPTooltipCreator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipCreator.OnPointerEnter(eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipCreator.OnPointerExit(eventData);
        }
    }
}