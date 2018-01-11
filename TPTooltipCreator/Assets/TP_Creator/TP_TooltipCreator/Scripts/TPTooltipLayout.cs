using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TP_Tooltip
{
    public class TPTooltipLayout : MonoBehaviour
    {
        [HideInInspector] public List<TextMeshProUGUI> Texts;
        [HideInInspector] public List<Image> Images;
        [HideInInspector] public List<Button> Buttons;

        Transform panelTranform;
        [HideInInspector] public Transform PanelTransform { get { return panelTranform; } private set { panelTranform = value; } }
        [HideInInspector] public Transform TextsParent;
        [HideInInspector] public Transform ImagesParent;
        [HideInInspector] public Transform ButtonsParent;

        void OnValidate()
        {
            Awake();
        }
        void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            PanelTransform = transform.GetChild(0);
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

        public void SetText(int index, string text) { Set(index, text, Texts); }
        public void SetText(int[] indexes, string text) { Set(indexes, text, Texts); }
        public void SetText(int[] indexes, string[] texts) { Set(indexes, texts, Texts); }
        public void SetText(TextMeshProUGUI Text, string text) { Set(Text, text, Texts); }

        public void SetImage(int index, Sprite sprite) { Set(index, sprite, Images); }
        public void SetImage(int[] indexes, Sprite sprite) { Set(indexes, sprite, Images); }
        public void SetImage(int[] indexes, Sprite[] sprites) { Set(indexes, sprites, Images); }
        public void SetImage(Image image, Sprite sprite) { Set(image, sprite, Images); }

        public void SetButtonClick(int index, UnityEngine.Events.UnityAction action) { Set(index, action, Buttons); }
        public void SetButtonClick(int[] indexes, UnityEngine.Events.UnityAction action) { Set(indexes, action, Buttons); }
        public void SetButtonClick(int[] indexes, UnityEngine.Events.UnityAction[] actions) { Set(indexes, actions, Buttons); }
        public void SetButtonClick(Button button, UnityEngine.Events.UnityAction action) { Set(button, action, Buttons); }

        // **** List's obj Getters ****//
        public TextMeshProUGUI GetText(int index)
        {
            return Texts[index];
        }
        public Image GetImage(int index)
        {
            return Images[index];
        }
        public Button GetButton(int index)
        {
            return Buttons[index];
        }

        // **** List's obj Setters ****//
        void Set<T, U>(int index, T obj, List<U> list)
        {
            System.Type Text = typeof(List<TextMeshProUGUI>);
            System.Type Image = typeof(List<Image>);
            System.Type Button = typeof(List<Button>);
            System.Type List = list.GetType();
            
            if (List == Text)
            {
                (list[index] as TextMeshProUGUI).text = obj as string;
            }
            else if (List == Image)
            {
                (list[index] as Image).sprite = obj as Sprite;
            }
            else if (List == Button)
            {
                (list[index] as Button).onClick.AddListener(obj as UnityEngine.Events.UnityAction);
            }
        }

        void Set<T, U>(int[] indexes, T obj, List<U> list)
        {
            int indexesLength = indexes.Length;
            int length = Texts.Count;

            for (int i = 0; i < length && i < indexesLength; i++)
            {
                Set(indexes[i], obj, list);
            }
        }

        void Set<T, U>(int[] indexes, T[] obj, List<U> list)
        {
            int indexesLength = indexes.Length;
            int objLength = obj.Length;
            int length = Texts.Count;
            int lastObjIndex = objLength - 1;

            for (int i = 0; i < length && i < indexesLength; i++)
            {
                if (i >= objLength)
                    Set(indexes[i], obj[lastObjIndex], list);
                else
                    Set(indexes[i], obj[i], list);
            }
        }

        void Set<T, D, U>(D type, T obj, List<U> list)
        {
            int index = -1;
            int length = Texts.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i].Equals(type))
                    index = i;
            }

            Set(index, obj, list);
        }
    }
}