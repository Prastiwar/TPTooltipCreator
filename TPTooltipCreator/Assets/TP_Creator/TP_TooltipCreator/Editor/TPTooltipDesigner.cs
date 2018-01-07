using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    //public static TPTooltipGUIData EditorData;
    //public static TPTooltipCreator TooltipCreator;
    GUISkin skin;

    Texture2D headerTexture;
    Texture2D managerTexture;
    Texture2D toolTexture;

    Rect headerSection;
    Rect managerSection;
    Rect toolSection;

    void OnEnable()
    {
        InitEditorData();
        InitTextures();
        InitCreator();
    }

    void InitEditorData()
    {
        //EditorData = AssetDatabase.LoadAssetAtPath(
        //       "Assets/TP_Creator/TP_InventoryCreator/EditorResources/EditorGUIData.asset",
        //       typeof(TPInventoryGUIData)) as TPInventoryGUIData;

        //if (EditorData == null)
        //{
        //    Debug.Log("Editor Data didn't found! Check path: 'Assets/TP_Creator/TP_InventoryCreator/EditorResources/EditorGUIData.asset'");
        //    return;
        //}
        //skin = EditorData.GUISkin;
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
        //if (InventoryCreator == null)
        //{
        //    InventoryCreator = FindObjectOfType<TPInventoryCreator>();

        //    if (InventoryCreator != null)
        //        UpdateManager();
        //}

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

        //if (InventoryCreator == null)
        //{
        //    InitializeManager();
        //}
        //else
        //{
        //    ChangeParent();
        //    SpawnEmpty();
        //    ResetManager();

        //    if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(60)))
        //    {
        //        UpdateManager();
        //    }
        //}

        GUILayout.EndArea();
    }

    void InitializeManager()
    {
        //if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(50)))
        //{
        //    GameObject go = (new GameObject("TP_InventoryManager", typeof(TPInventoryCreator)));
        //    InventoryCreator = go.GetComponent<TPInventoryCreator>();
        //    UpdateManager();
        //    Debug.Log("Inventory Manager created!");
        //}

        //if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(50)))
        //    existManager = !existManager;

        //if (existManager)
        //    InventoryCreator = EditorGUILayout.ObjectField(InventoryCreator, typeof(TPInventoryCreator), true,
        //        GUILayout.Height(30)) as TPInventoryCreator;
    }

    void DrawTools()
    {
        GUILayout.BeginArea(toolSection);
        GUILayout.Label("Inventory Manager - Tools", skin.box);
        if (GUILayout.Button("placeholder", skin.button, GUILayout.Height(50)))
        {
            //TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Items);
        }
        if (GUILayout.Button("placeholder", skin.button, GUILayout.Height(50)))
        {
            //TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Types);
        }
        if (GUILayout.Button("placeholder", skin.button, GUILayout.Height(50)))
        {
            //TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Stats);
        }
        if (GUILayout.Button("placeholder", skin.button, GUILayout.Height(50)))
        {
            //TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Slots);
        }
        GUILayout.EndArea();
    }

}