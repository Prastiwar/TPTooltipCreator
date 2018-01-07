using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_TooltipCreator;

namespace TP_TooltipEditor
{
    public class TPTooltipDesigner : EditorWindow
    {
        [MenuItem("TP_Creator/TP_TooltipCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Inventory Designer runtime!");
                return;
            }
            TPTooltipDesigner window = (TPTooltipDesigner)GetWindow(typeof(TPTooltipDesigner));
            window.minSize = new Vector2(615, 330);
            window.maxSize = new Vector2(615, 330);
            window.Show();
        }

        public static TPTooltipGUIData EditorData;
        public static TPTooltipCreator TooltipCreator;
        GUISkin skin;

        Texture2D headerTexture;
        Texture2D managerTexture;
        Texture2D toolTexture;

        Rect headerSection;
        Rect managerSection;
        Rect toolSection;

        bool existManager;
        bool toggleChange;

        SerializedObject creator;
        SerializedProperty tooltipLayout;

        void OnEnable()
        {
            InitEditorData();
            InitTextures();
            InitCreator();

            creator = new SerializedObject(TooltipCreator);
            tooltipLayout = creator.FindProperty("TooltipLayout");
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_TooltipCreator/EditorResources/EditorGUIData.asset",
                   typeof(TPTooltipGUIData)) as TPTooltipGUIData;

            if (EditorData == null)
            {
                Debug.Log("Editor Data didn't found! Check path: 'Assets/TP_Creator/TP_TooltipCreator/EditorResources/EditorGUIData.asset'");
                return;
            }
            skin = EditorData.GUISkin;
        }

        void InitTextures()
        {
            Color colorHeader = new Color(0.19f, 0.19f, 0.19f);
            Color color = new Color(0.15f, 0.15f, 0.15f);

            headerTexture = new Texture2D(1, 1);
            headerTexture.SetPixel(0, 0, colorHeader);
            headerTexture.Apply();

            managerTexture = new Texture2D(1, 1);
            managerTexture.SetPixel(0, 0, color);
            managerTexture.Apply();

            toolTexture = new Texture2D(1, 1);
            toolTexture.SetPixel(0, 0, color);
            toolTexture.Apply();
        }

        static void InitCreator()
        {
            if (TooltipCreator == null)
            {
                TooltipCreator = FindObjectOfType<TPTooltipCreator>();

                if (TooltipCreator != null)
                    UpdateManager();
            }

            //var data = AssetDatabase.LoadAssetAtPath("Assets/" + EditorData.InventoryDataPath + "InventoryData" + ".asset",
            //    typeof(TPInventoryData));

            //if (data == null)
            //    CreateInventoryData();
            //else
            //    (data as TPInventoryData).Refresh();
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                //if (TPInventoryToolsWindow.window)
                //TPInventoryToolsWindow.window.Close();
                this.Close();
            }
            DrawLayouts();
            DrawHeader();
            DrawManager();
            DrawTools();
        }
        void DrawLayouts()
        {
            headerSection = new Rect(0, 0, Screen.width, 50);
            managerSection = new Rect(0, 50, Screen.width / 2, Screen.height);
            toolSection = new Rect(Screen.width / 2, 50, Screen.width / 2, Screen.height);

            GUI.DrawTexture(headerSection, headerTexture);
            GUI.DrawTexture(managerSection, managerTexture);
            GUI.DrawTexture(toolSection, toolTexture);
        }

        void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Label("TP Tooltip Creator - Manage your Tooltip!", skin.GetStyle("HeaderLabel"));
            GUILayout.EndArea();
        }

        void DrawManager()
        {
            GUILayout.BeginArea(managerSection);
            GUILayout.Label("Tooltip Manager - Core", skin.box);

            if (TooltipCreator == null)
            {
                InitializeManager();
            }
            else
            {
                ChangeTooltipLayout();
                SpawnEmpty();
                ResetManager();

                if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(60)))
                {
                    UpdateManager();
                }
            }

            GUILayout.EndArea();
        }

        void InitializeManager()
        {
            if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(50)))
            {
                GameObject go = (new GameObject("TP_TooltipManager", typeof(TPTooltipCreator)));
                TooltipCreator = go.GetComponent<TPTooltipCreator>();
                UpdateManager();
                Debug.Log("Tooltip Manager created!");
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(50)))
                existManager = !existManager;

            if (existManager)
                TooltipCreator = EditorGUILayout.ObjectField(TooltipCreator, typeof(TPTooltipCreator), true,
                    GUILayout.Height(30)) as TPTooltipCreator;
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(35)))
                TooltipCreator = null;
        }

        void SpawnEmpty()
        {
            if (GUILayout.Button("Spawn empty tooltip", skin.button, GUILayout.Height(40)))
            {
                if (EditorData.TooltipPrefab == null)
                {
                    Debug.LogError("There is no tooltip prefab in EditorGUIData file!");
                    return;
                }
                Instantiate(EditorData.TooltipPrefab);
                Debug.Log("Tooltip example Created");
            }
        }

        void ChangeTooltipLayout()
        {
            if (GUILayout.Button("Change Tooltip Layout", skin.button, GUILayout.Height(50)))
                toggleChange = !toggleChange;

            if (toggleChange)
            {
                EditorGUILayout.LabelField("Put there parent prefab of your tooltip layout", skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(tooltipLayout, GUIContent.none, GUILayout.Height(15));

                if (GUI.changed)
                {
                    UpdateManager();
                }
                EditorGUILayout.Space();
            }
        }

        public static void UpdateManager()
        {
            TooltipCreator.Refresh();
        }

        void DrawTools()
        {
            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Inventory Manager - Tools", skin.box);
            if (GUILayout.Button("Preview Offset", skin.button, GUILayout.Height(70)))
            {
                TPTooltipToolsWindow.OpenToolWindow(TPTooltipToolsWindow.ToolEnum.Preview);
            }
            if (GUILayout.Button("Observers", skin.button, GUILayout.Height(70)))
            {
                TPTooltipToolsWindow.OpenToolWindow(TPTooltipToolsWindow.ToolEnum.Observers);
            }
            GUILayout.EndArea();
        }

    }
}