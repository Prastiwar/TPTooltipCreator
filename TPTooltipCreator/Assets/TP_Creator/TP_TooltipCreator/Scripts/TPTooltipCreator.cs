using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace TP_TooltipCreator
{
    public class TPTooltipCreator : MonoBehaviour
    {
        public GameObject TooltipLayout;
        public Vector2 Offset;
        public TPTooltipObserver OnObserver;
        public List<TPTooltipObserver> Observers = new List<TPTooltipObserver>();

        Image PanelImage;
        Transform Panel;
        Vector2 PanelVector2;
        PointerEventData _eventData;
        WaitWhile Wait;

        [SerializeField] List<GameObject> GMObservers;

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

            Observers.Clear();

            foreach (var Observer in GMObservers)
            {
                if (Observer != null)
                {
                    if (Observer.GetComponent<TPTooltipObserver>())
                        Observers.Add(Observer.GetComponent<TPTooltipObserver>());
                    else
                        Observers.Add(Observer.gameObject.AddComponent<TPTooltipObserver>());
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData, TPTooltipObserver.ToolTipType Type)
        {
            OnObserver = eventData.pointerEnter.GetComponent<TPTooltipObserver>();
            _eventData = eventData;
            Animate(true);

            if(Type == TPTooltipObserver.ToolTipType.Dynamic)
                StartCoroutine(ToolTipPositioning());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnObserver = null;
            _eventData = null;
            Animate(false);
        }

        IEnumerator ToolTipPositioning()
        {
            while (_eventData != null)
            {
                PanelVector2 = _eventData.position + Offset;
                Panel.position = PanelVector2;
                yield return Wait;
            }
        }

        public virtual void Animate(bool SetActive)
        {
            TooltipLayout.SetActive(SetActive);
        }
    }
}