using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace TP_TooltipCreator
{
    public class TPTooltipCreator : MonoBehaviour
    {
        [HideInInspector] public TPTooltipLayout TooltipLayout;
        [HideInInspector] public Transform StaticTransform;
        [HideInInspector] public Vector2 Offset;
        [HideInInspector] public TPTooltipObserver OnObserver;
        [HideInInspector] public List<TPTooltipObserver> Observers = new List<TPTooltipObserver>();

        [SerializeField] List<GameObject> OBJObservers;
        PointerEventData _eventData;
        WaitWhile Wait;
        GameObject TooltipLayoutCanvas;
        Vector2 PanelVector2;
        float PanelHeight2;
        float PanelWidth2;
        UnityEngine.Events.UnityAction OnEnterObserver;
        UnityEngine.Events.UnityAction OnAnimate;

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
                TooltipLayoutCanvas = TooltipLayout.gameObject;
                Image PanelImage = TooltipLayout.PanelTransform.GetComponent<Image>();
                Rect PanelRect = PanelImage.rectTransform.rect;
                PanelWidth2 = PanelRect.width / 2;
                PanelHeight2 = PanelRect.height / 2;
            }

            Observers.Clear();

            foreach (var Observer in OBJObservers)
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData == null)
                return;

            OnObserver = eventData.pointerEnter.GetComponent<TPTooltipObserver>();
            _eventData = eventData;
            Animate(true);

            if (OnObserver == null)
                return;

            if (OnObserver.SetType == TPTooltipObserver.ToolTipType.DynamicEnter ||
                OnObserver.SetType == TPTooltipObserver.ToolTipType.DynamicClick)
                StartCoroutine(ToolTipPositioning());
            else
                TooltipLayout.PanelTransform.position = StaticTransform.position;

            OnEnterObserver();
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
                PanelVector2.Set(Mathf.Clamp(PanelVector2.x, PanelWidth2, Screen.width - PanelWidth2),
                    Mathf.Clamp(PanelVector2.y, PanelHeight2, Screen.height - PanelHeight2));

                TooltipLayout.PanelTransform.position = PanelVector2;
                yield return Wait;
            }
        }

        void Animate(bool SetActive)
        {
            if (TooltipLayout == null)
            {
                Debug.LogError("No Layout loaded");
                return;
            }

            if (OnAnimate != null)
                OnAnimate();
            else
                SetTooltipActive(SetActive);
        }

        public void SetOnEnterObserver(UnityEngine.Events.UnityAction action)
        {
            OnEnterObserver = action;
        }

        public void SetTooltipActive(bool SetActive)
        {
            TooltipLayoutCanvas.SetActive(SetActive);
        }

        public void SetAnimation(UnityEngine.Events.UnityAction action)
        {
            OnAnimate = action;
        }
    }
}