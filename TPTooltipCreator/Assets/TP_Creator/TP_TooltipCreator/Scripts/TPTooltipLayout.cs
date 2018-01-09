using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TP_TooltipCreator
{
    public class TPTooltipLayout : MonoBehaviour
    {
        public List<TextMeshProUGUI> Texts;
        public List<Image> Images;
        public List<Button> Buttons;

        public Transform PanelTransform;
        public Transform TextsParent;
        public Transform ImagesParent;
        public Transform ButtonsParent;

        //TPTooltipCreator creator;

        void OnValidate()
        {
            Awake();
        }
        void Awake()
        {
            //creator = FindObjectOfType<TPTooltipCreator>();

            Refresh();
        }

        public void Refresh()
        {
            Texts.Clear();
            Images.Clear();
            Buttons.Clear();
            GetComponents(TextsParent, Texts);
            GetComponents(ImagesParent, Images);
            GetComponents(ButtonsParent, Buttons);
        }

        void GetComponents<T>(Transform child, List<T> list)
        {
            if (child == null)
                return;

            foreach (Transform item in child)
            {
                list.Add(item.GetComponent<T>());
            }
        }


    }
}