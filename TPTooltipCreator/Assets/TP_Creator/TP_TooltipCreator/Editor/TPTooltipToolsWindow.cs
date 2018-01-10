using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TP_TooltipCreator;

namespace TP_TooltipEditor
{
    public class TPTooltipToolsWindow : EditorWindow
    {
        public static TPTooltipToolsWindow window;
        public enum ToolEnum
        {
            Preview,
            Observers,
            Layout,
        }

        static ToolEnum tool;
        string[] enumNamesList = System.Enum.GetNames(typeof(TPTooltipObserver.ToolTipType));

        SerializedObject TooltipLayout;
        SerializedProperty tooltipLayout;
        SerializedProperty layoutTexts;
        SerializedProperty layoutImages;
        SerializedProperty layoutButtons;
        SerializedProperty layoutTextsParent;
        SerializedProperty layoutImagesParent;
        SerializedProperty layoutButtonsParent;
        
        SerializedProperty observerOBJList;
        SerializedProperty offset;

        GUIContent content = new GUIContent("You can drag there multiple observers   |  Size");

        Texture2D mainTexture;
        Texture2D tooltipTexture;
        Texture2D previewTexture;

        Vector2 scrollPos = Vector2.zero;
        Vector2 textureVec;

        Rect mainRect;
        Rect leftUp;
        Rect leftDown;
        Rect center;
        Rect rightUp;
        Rect rightDown;

        bool toggleItems = false;
        bool[] showBools = new bool[3];

        static float windowSize = 450;
        static float windowPreviewSize = 600;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            if (window != null)
                window.Close();

            tool = _tool;
            window = (TPTooltipToolsWindow)GetWindow(typeof(TPTooltipToolsWindow));
            if (tool == ToolEnum.Preview)
            {
                window.minSize = new Vector2(windowPreviewSize, windowPreviewSize);
                window.maxSize = new Vector2(windowPreviewSize, windowPreviewSize);
            }
            else
            {
                window.minSize = new Vector2(windowSize, windowSize);
                window.maxSize = new Vector2(windowSize, windowSize);
            }
            window.Show();
        }

        void OnEnable()
        {
            InitTextures();
            InitRects();
            if(TPTooltipDesigner.TooltipCreator.TooltipLayout)
                TooltipLayout = new SerializedObject(TPTooltipDesigner.TooltipCreator.TooltipLayout);
            observerOBJList = TPTooltipDesigner.creator.FindProperty("OBJObservers");
            offset = TPTooltipDesigner.creator.FindProperty("Offset");
            tooltipLayout = TPTooltipDesigner.creator.FindProperty("TooltipLayout");

            FindLayoutProperties();
        }

        void FindLayoutProperties()
        {
            if (TooltipLayout == null)
                return;

            layoutTexts = TooltipLayout.FindProperty("Texts");
            layoutImages = TooltipLayout.FindProperty("Images");
            layoutButtons = TooltipLayout.FindProperty("Buttons");
            layoutTextsParent = TooltipLayout.FindProperty("TextsParent");
            layoutImagesParent = TooltipLayout.FindProperty("ImagesParent");
            layoutButtonsParent = TooltipLayout.FindProperty("ButtonsParent");

            toggleItems = tooltipLayout.objectReferenceValue != null ? true : false;
        }

        void InitRects()
        {
            float boxSize = 70;
            float size = windowPreviewSize - boxSize;
            float gap = 20;

            leftUp = new Rect(gap, gap, boxSize, boxSize);
            leftDown = new Rect(gap, size - 60, boxSize, boxSize);
            center = new Rect((size - gap) / 2, (size - boxSize) / 2, boxSize, boxSize);
            rightUp = new Rect(size - gap, gap, boxSize, boxSize);
            rightDown = new Rect(size - gap, size - 60, boxSize, boxSize);
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();

            InitPreviewTextures();
        } 

        void InitPreviewTextures()
        {
            if (TPTooltipDesigner.TooltipCreator.TooltipLayout == null)
            {
                if(tool == ToolEnum.Preview) Debug.LogError("No layout loaded! Change it in 'Layout' tool");
                return;
            }

            previewTexture = new Texture2D(100, 100);
            previewTexture.SetPixel(0, 0, Color.red);
            previewTexture.Apply();
            previewTexture = AssetDatabase.LoadAssetAtPath(
                "Assets/TP_Creator/TP_TooltipCreator/EditorResources/preview.png", typeof(Texture2D)) as Texture2D;

            var panel = TPTooltipDesigner.TooltipCreator.TooltipLayout.PanelTransform.GetComponent<RectTransform>().rect;
            tooltipTexture = new Texture2D((int)panel.width, (int)panel.height);
            tooltipTexture.SetPixel(0, 0, Color.white);
            tooltipTexture.Apply();
            textureVec = new Vector2(tooltipTexture.width, tooltipTexture.height);
        }

        void OnGUI()
        {
            mainRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(mainRect, mainTexture);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));
            DrawTool();
            GUILayout.EndScrollView();
        }

        void DrawTool()
        {
            switch (tool)
            {
                case ToolEnum.Preview:
                    DrawPreviewTool();
                    break;
                case ToolEnum.Observers:
                    DrawObserverTool();
                    break;
                case ToolEnum.Layout:
                    DrawLayoutsTool();
                    break;
                default:
                    break;
            }
        }
        
        void DrawObserverTool()
        {
            if (observerOBJList == null)
                return;

            if (GUILayout.Button("Add new", TPTooltipDesigner.EditorData.GUISkin.button))
            {
                AddObserver();
            }
            if (GUILayout.Button("Automatically find all Observer's", TPTooltipDesigner.EditorData.GUISkin.button))
            {
                AutoFindObservers();
            }
            if (observerOBJList.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No observers loaded!", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Observers loaded:", GUILayout.Width(180));

            TPTooltipDesigner.creator.Update();
            observerOBJList.serializedObject.Update();
            ShowObservers(observerOBJList);
        }

        void AutoFindObservers()
        {
            TPTooltipObserver[] observersFound = FindObjectsOfType<TPTooltipObserver>();
            int length = observersFound.Length;
            observerOBJList.arraySize = length;
            for (int i = 0; i < length; i++)
            {
                observerOBJList.GetArrayElementAtIndex(i).objectReferenceValue = observersFound[i].gameObject;
                observerOBJList.serializedObject.ApplyModifiedProperties();
            }
        }

        void ShowObservers(SerializedProperty list)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(list, content, false);
            if (Event.current.type == EventType.DragPerform && DragAndDrop.objectReferences.Length > 1)
                return;

            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"), GUIContent.none, GUILayout.Width(90));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(175));
            EditorGUILayout.LabelField("Is Observing?");
            GUILayout.EndHorizontal();
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                ToggleObserving(list, i);
                Check(list, i);
                SetType(list, i);
                EditAsset(list, i);
                RemoveAsset(list, i);
                GUILayout.EndHorizontal();
            }
            if (GUI.changed)
                observerOBJList.serializedObject.ApplyModifiedProperties();
        }

        void ToggleObserving(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue == null ||
                (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>() == null)
                return;

            (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>().IsObserving =
                EditorGUILayout.Toggle(
                    (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>().IsObserving,
                    GUILayout.Width(15));

            if (GUI.changed)
                observerOBJList.serializedObject.ApplyModifiedProperties();
        }

        void Check(SerializedProperty list, int index)
        {
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                if (i == index)
                    continue;
                if (list.GetArrayElementAtIndex(index).objectReferenceValue == list.GetArrayElementAtIndex(i).objectReferenceValue)
                {
                    list.GetArrayElementAtIndex(i).objectReferenceValue = null;
                }
            }
        }

        void RemoveAsset(SerializedProperty list, int index)
        {
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                if (list.GetArrayElementAtIndex(index).objectReferenceValue != null || index == list.arraySize - 1)
                {
                    if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                    {
                        TPTooltipObserver script = (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>();
                        DestroyImmediate(script);
                        list.GetArrayElementAtIndex(index).objectReferenceValue = null;
                    }
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }

        void AddObserver()
        {
            observerOBJList.arraySize++;
            observerOBJList.serializedObject.ApplyModifiedProperties();
            TPTooltipDesigner.UpdateManager();
        }

        void EditAsset(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                if (GUILayout.Button("Edit", GUILayout.Width(35)))
                {
                    AssetDatabase.OpenAsset(list.GetArrayElementAtIndex(index).objectReferenceValue);
                }
        }

        void SetType(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue == null)
                return;
            if ((list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>() == null)
                return;

            int actualSelected = 1;
            int selectionFromInspector =
                (int)(list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>().SetType;

            actualSelected = EditorGUILayout.Popup( selectionFromInspector, enumNamesList, GUILayout.Width(100));

            (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>().SetType
                = (TPTooltipObserver.ToolTipType)actualSelected;
        }

        void DrawPreviewTool()
        {
            if (TPTooltipDesigner.TooltipCreator.TooltipLayout == null)
                return;
            EditorGUILayout.PropertyField(offset);
            offset.serializedObject.ApplyModifiedProperties();
            GUILayout.BeginArea(new Rect(0, 50, Screen.width, Screen.height));
            GUI.DrawTexture(leftUp, previewTexture);
            GUI.DrawTexture(leftDown, previewTexture);

            GUI.DrawTexture(center, previewTexture);

            GUI.DrawTexture(rightUp, previewTexture);
            GUI.DrawTexture(rightDown, previewTexture);

            Event e = Event.current;
            if (leftUp.Contains(e.mousePosition) || leftDown.Contains(e.mousePosition) ||
                center.Contains(e.mousePosition) || rightUp.Contains(e.mousePosition) || rightDown.Contains(e.mousePosition))
            {
                Vector2 pos = (e.mousePosition - (textureVec / 2)) + offset.vector2Value;
                pos.Set(Mathf.Clamp(pos.x, 0, window.maxSize.x - (tooltipTexture.width)),
                    Mathf.Clamp(pos.y, 0, window.maxSize.y - (tooltipTexture.height + 50)));
                Rect rect = new Rect(pos, textureVec);
                GUI.DrawTexture(rect, tooltipTexture);
            }

            GUILayout.EndArea();
        }

        void Update()
        {
            if(tool == ToolEnum.Preview)
                Repaint();
        }

        void DrawLayoutsTool()
        {
            EditorGUILayout.LabelField("Put there parent of your Tooltip Layout", TPTooltipDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(tooltipLayout, GUIContent.none, GUILayout.Height(30));

            if (GUI.changed)
            {
                TPTooltipDesigner.UpdateManager();
                if (TPTooltipDesigner.TooltipCreator.TooltipLayout)
                    TooltipLayout = new SerializedObject(TPTooltipDesigner.TooltipCreator.TooltipLayout);
                tooltipLayout.serializedObject.ApplyModifiedProperties();
                toggleItems = tooltipLayout.objectReferenceValue != null ? true : false;
            }

            EditorGUILayout.Space();

            if (toggleItems)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Show Texts"))
                    ToggleShow(0);
                if (GUILayout.Button("Show Images"))
                    ToggleShow(1);
                if (GUILayout.Button("Show Buttons"))
                    ToggleShow(2);
                EditorGUILayout.EndHorizontal();

                if (layoutTextsParent == null || layoutImagesParent == null || layoutButtonsParent == null)
                {
                    FindLayoutProperties();
                    return;
                }

                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.PropertyField(layoutTextsParent, GUIContent.none);
                EditorGUILayout.PropertyField(layoutImagesParent, GUIContent.none);
                EditorGUILayout.PropertyField(layoutButtonsParent, GUIContent.none);

                if (GUI.changed)
                {
                    TooltipLayout.ApplyModifiedProperties();
                    TPTooltipDesigner.UpdateManager();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (showBools[0])
                    DrawItem(layoutTexts);
                if (showBools[1])
                    DrawItem(layoutImages);
                if (showBools[2])
                    DrawItem(layoutButtons);
            }
        }

        void ToggleShow(int showIndex)
        {
            int length = showBools.Length;
            for (int i = 0; i < length; i++)
                showBools[i] = false;

            showBools[showIndex] = true;
        }
        
        void DrawItem(SerializedProperty layout)
        {
            layout.serializedObject.Update();

            if (layout.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Nothing loaded!", MessageType.Error);
                return;
            }
            int length = layout.arraySize;
            for (int i = 0; i < length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(layout.GetArrayElementAtIndex(i), GUIContent.none, GUILayout.Width(350));
                EditAsset(layout, i);
                GUILayout.EndHorizontal();
            }
        }

    } 
}