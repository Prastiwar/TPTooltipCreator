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

        SerializedProperty observerList;
        SerializedProperty offset;
        SerializedProperty tooltipLayout;

        GUIContent content = new GUIContent("You can drag there multiple observers   |  Size");

        Texture2D mainTexture;
        Texture2D tooltipTexture;
        Texture2D previewTexture;

        Vector2 scrollPos = Vector2.zero;
        Vector2 textureVec;

        Rect mainRect;
        Rect leftUp = new Rect(10, 10, 50, 50);
        Rect leftDown = new Rect(10, 300, 50, 50);
        Rect center = new Rect(175, 175, 50, 50);
        Rect rightUp = new Rect(340, 10, 50, 50);
        Rect rightDown = new Rect(340, 300, 50, 50);

        public static void OpenToolWindow(ToolEnum _tool)
        {
            window = (TPTooltipToolsWindow)GetWindow(typeof(TPTooltipToolsWindow));
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(400, 400);
            window.Show();
            tool = _tool;
        }

        void OnEnable()
        {
            InitTextures();
            observerList = TPTooltipDesigner.creator.FindProperty("GMObservers");
            offset = TPTooltipDesigner.creator.FindProperty("Offset");
            tooltipLayout = TPTooltipDesigner.creator.FindProperty("TooltipLayout");
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();

            previewTexture = new Texture2D(100, 100);
            previewTexture.SetPixel(0, 0, Color.red);
            previewTexture.Apply();

            InitPreviewTexture();
            
        } 

        void InitPreviewTexture()
        {
            if (tool != ToolEnum.Preview)
                return;

            if (TPTooltipDesigner.TooltipCreator.TooltipLayout == null)
            {
                Debug.LogError("No layout loaded! Change it in 'Layout' tool");
                return;
            }
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
            if (GUILayout.Button("Add new", TPTooltipDesigner.EditorData.GUISkin.button))
            {
                AddObserver();
            }
            if (observerList.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No observers loaded!", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Observers loaded:", GUILayout.Width(180));

            TPTooltipDesigner.creator.Update();
            observerList.serializedObject.Update();
            ShowObservers(observerList);
            observerList.serializedObject.ApplyModifiedProperties();
            TPTooltipDesigner.creator.ApplyModifiedProperties();
        }

        void ShowObservers(SerializedProperty list)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(list, content, false);
            if (Event.current.type == EventType.DragPerform && DragAndDrop.objectReferences.Length > 1)
                return;

            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"), GUIContent.none, GUILayout.Width(90));
            GUILayout.EndHorizontal();
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                Check(list, i);
                SetType(list, i);
                EditAsset(list, i);
                RemoveAsset(list, i);
                GUILayout.EndHorizontal();
            }
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
                    list.DeleteArrayElementAtIndex(index);
            }
        }

        void AddObserver()
        {
            observerList.arraySize++;
            observerList.serializedObject.ApplyModifiedProperties();
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

            actualSelected = EditorGUILayout.Popup( selectionFromInspector, enumNamesList, GUILayout.Width(70));

            (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>().SetType
                = actualSelected == 1 ? TPTooltipObserver.ToolTipType.Static : TPTooltipObserver.ToolTipType.Dynamic;
        }

        void DrawPreviewTool()
        {
            EditorGUILayout.PropertyField(offset);
            offset.serializedObject.ApplyModifiedProperties();
            GUILayout.BeginArea(new Rect(0, 50, Screen.width, Screen.height));
            GUI.DrawTexture(leftUp, previewTexture);
            GUI.DrawTexture(leftDown, previewTexture);

            GUI.DrawTexture(center, previewTexture);

            GUI.DrawTexture(rightUp, previewTexture);
            GUI.DrawTexture(rightDown, previewTexture);

            Event e = Event.current;
            Vector2 pos = (e.mousePosition - (textureVec / 2)) + offset.vector2Value;
            pos.Set(Mathf.Clamp(pos.x, 0, window.maxSize.x - (tooltipTexture.width)),
                Mathf.Clamp(pos.y, 0, window.maxSize.y - (tooltipTexture.height * 1.5f)));
            Rect rect = new Rect(pos, textureVec);
            GUI.DrawTexture(rect, tooltipTexture);
            GUILayout.EndArea();
        }

        void Update()
        {
            if(tool == ToolEnum.Preview)
                Repaint();
        }

        void DrawLayoutsTool()
        {
            EditorGUILayout.LabelField("Put there parent of your Tooltip Layout)", TPTooltipDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(tooltipLayout, GUIContent.none, GUILayout.Height(15));
            TPTooltipDesigner.creator.ApplyModifiedProperties();

            if (GUI.changed)
                TPTooltipDesigner.UpdateManager();
            EditorGUILayout.Space();
        }

    }
}