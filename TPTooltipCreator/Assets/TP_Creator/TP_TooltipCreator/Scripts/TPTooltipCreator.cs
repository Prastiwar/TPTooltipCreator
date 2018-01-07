using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace TP_TooltipCreator
{
    public class TPTooltipCreator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public List<TPTooltipObserver> Observers;
        public GameObject TooltipLayout;
        public TPTooltipObserver OnObserver;

        Image PanelImage;
        Transform Panel;
        Vector2 PanelVector2;
        PointerEventData _eventData;
        WaitWhile Wait;


        void OnValidate()
        {
            Awake();
        }
        void Awake()
        {
            Refresh();
            if (Wait == null && _eventData != null) Wait = new WaitWhile(() => PanelVector2 == _eventData.position);
        }

        public void Refresh()
        {
            if (TooltipLayout != null)
            {
                if (PanelImage == null) PanelImage = TooltipLayout.GetComponentInChildren<Image>();
                if (Panel == null) Panel = PanelImage.transform;
            }

            foreach (var Observer in Observers)
            {
                if (!Observer.GetComponent<TPTooltipObserver>()) // doesnt work
                    Observer.gameObject.AddComponent<TPTooltipObserver>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //if(eventData.pointerEnter.GetComponent<TPTooltipObserver>() != null)
            OnObserver = eventData.pointerEnter.GetComponent<TPTooltipObserver>();
            _eventData = eventData;
            TooltipLayout.SetActive(true);
            StartCoroutine(ToolTipPositioning());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnObserver = null;
            _eventData = null;
            TooltipLayout.SetActive(false);
        }

        IEnumerator ToolTipPositioning()
        {
            while (_eventData != null)
            {
                PanelVector2 = _eventData.position;
                Panel.position = PanelVector2;
                yield return Wait;
            }
        }
    }
}