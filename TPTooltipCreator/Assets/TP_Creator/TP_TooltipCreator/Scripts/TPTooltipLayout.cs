using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TP.Tooltip
{
    public class TPTooltipLayout : MonoBehaviour
    {
        public List<TextMeshProUGUI> Texts;
        public List<Image> Images;
        public List<Button> Buttons;

        Transform panelTranform;
        [HideInInspector] public Transform PanelTransform { get { return panelTranform; } private set { panelTranform = value; } }
        public Transform TextsParent;
        public Transform ImagesParent;
        public Transform ButtonsParent;

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
            if(PanelTransform == null) PanelTransform = transform.GetChild(0);
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

        public void SetText(string text, int index) { Set(index, text, Texts); }
        public void SetText(string text, params int[] indexes) { Set(indexes, text, Texts); }
        public void SetText(string[] texts, params int[] indexes) { Set(indexes, texts, Texts); }
        public void SetText(string text, TextMeshProUGUI Text) { Set(Text, text, Texts); }
        public void SetText(TextMeshProUGUI[] TextMeshes, params string[] texts) { Set(TextMeshes, texts, Texts); }

        public void SetImage(Sprite sprite, int index) { Set(index, sprite, Images); }
        public void SetImage(Sprite sprite, params int[] indexes) { Set(indexes, sprite, Images); }
        public void SetImage(Sprite[] sprites, params int[] indexes) { Set(indexes, sprites, Images); }
        public void SetImage(Sprite sprite, Image image) { Set(image, sprite, Images); }
        public void SetImage(Image[] images, params Sprite[] sprites) { Set(images, sprites, Images); }

        public void SetButtonClick( UnityEngine.Events.UnityAction action, int index) { Set(index, action, Buttons); }
        public void SetButtonClick(UnityEngine.Events.UnityAction action, params int[] indexes) { Set(indexes, action, Buttons); }
        public void SetButtonClick(UnityEngine.Events.UnityAction[] actions, params int[] indexes) { Set(indexes, actions, Buttons); }
        public void SetButtonClick(UnityEngine.Events.UnityAction action, Button button) { Set(button, action, Buttons); }
        public void SetButtonClick(Button[] buttons, params UnityEngine.Events.UnityAction[] actions) { Set(buttons, actions, Buttons); }

        // **** List's obj Getters ****//
        public TextMeshProUGUI[] GetText(params int[] indexes)
        {
            int length = indexes.Length;
            TextMeshProUGUI[] _texts = new TextMeshProUGUI[length];

            for (int i = 0; i < length; i++)
                _texts[i] = Texts[indexes[i]];

            return _texts;
        }

        public Image[] GetImage(params int[] indexes)
        {
            int length = indexes.Length;
            Image[] _images = new Image[length];

            for (int i = 0; i < length; i++)
                _images[i] = Images[indexes[i]];

            return _images;
        }

        public Button[] GetButton(params int[] indexes)
        {
            int length = indexes.Length;
            Button[] _buttons = new Button[length];

            for (int i = 0; i < length; i++)
                _buttons[i] = Buttons[indexes[i]];

            return _buttons;
        }

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

        void Set<T, D, U>(D[] types, T obj, List<U> list)
        {
            int indexesLength = types.Length;
            int length = Texts.Count;

            for (int i = 0; i < length && i < indexesLength; i++)
            {
                Set(types[i], obj, list);
            }
        }

        void Set<T, D, U>(D[] types, T[] objs, List<U> list)
        {
            int typessLength = types.Length;
            int objsLength = objs.Length;
            int length = Texts.Count;
            int lastObjIndex = objsLength - 1;

            for (int i = 0; i < length && i < typessLength; i++)
            {
                if (i >= objsLength)
                    Set(types[i], objs[lastObjIndex], list);
                else
                    Set(types[i], objs[i], list);
            }
        }
    }
}