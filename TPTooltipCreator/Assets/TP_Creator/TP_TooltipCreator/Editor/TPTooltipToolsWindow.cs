using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Observers
        }

        static ToolEnum tool;
        //static object _object;
        //static string loaded;
        //static string noLoaded;
        //static string horizontalVar;
        //static Array array;
        //static UnityAction action;
        //static Type type;

        Rect mainRect;
        Vector2 scrollPos = Vector2.zero;
        Texture2D mainTexture;

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
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
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
            if (tool == ToolEnum.Preview)
            {
                DrawPreview();
            }
            else
            {
                DrawObserverTool();
            }
        }

        void DrawObserverTool()
        {
            if (GUILayout.Button("Add new", TPTooltipDesigner.EditorData.GUISkin.button))
            {
                AddObserver();
            }
            if (TPTooltipDesigner.TooltipCreator.Observers.Count == 0)
            {
                EditorGUILayout.HelpBox("No observers loaded!", MessageType.Error);
                return;
            }
            
            EditorGUILayout.LabelField("Observers loaded:", GUILayout.Width(180));

            foreach (var observer in TPTooltipDesigner.TooltipCreator.Observers)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(observer, typeof(GameObject), true, GUILayout.Width(200));
                //DrawObservers(observer);
                EditAsset(observer);
                RemoveAsset(observer);
                GUILayout.EndHorizontal();
                //EditorUtility.SetDirty((observer as UnityEngine.Object) == null ? this : (observer as UnityEngine.Object));
            }
        }

        void AddObserver()
        {
            TPTooltipDesigner.TooltipCreator.Observers.Add(null);
            TPTooltipDesigner.UpdateManager();
            //observerObjs.Add();
            //observerObjs.Clear();
            //observerProps.Clear();
        }
        void RemoveObserver(TPTooltipObserver observer)
        {
            TPTooltipDesigner.TooltipCreator.Observers.Remove(observer);
        }

        void RemoveAsset(TPTooltipObserver observer)
        {
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveObserver(observer);
            }
        }

        //List<SerializedObject> observerObjs = new List<SerializedObject>();
        //List<SerializedProperty> observerProps = new List<SerializedProperty>();
        //int iterator = 0;

        //void DrawObservers(UnityEngine.Object _object)
        //{
        //    if (observerObjs.Count != TPTooltipDesigner.TooltipCreator.Observers.Count)
        //    {
        //        SerializedObject observerObj = new SerializedObject(_object);
        //        SerializedProperty observerProp = observerObj.FindProperty("IsEquipSlot");
        //        observerObjs.Add(observerObj);
        //        observerProps.Add(observerProp);

        //        EditorGUILayout.PropertyField(observerProp, GUIContent.none, GUILayout.Width(30));
        //        observerObj.ApplyModifiedProperties();
        //    }
        //    else
        //    {
        //        EditorGUILayout.PropertyField(observerProps[iterator], GUIContent.none, GUILayout.Width(30));
        //        observerObjs[iterator].ApplyModifiedProperties();
        //        iterator++;
        //        if (iterator >= observerObjs.Count)
        //            iterator = 0;
        //    }

        //}

        void EditAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Edit", GUILayout.Width(35)))
            {
                AssetDatabase.OpenAsset(obj);
            }
        }

        void DrawPreview()
        {

        }
    }
}