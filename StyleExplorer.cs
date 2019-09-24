using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class StringExtensions
{
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}

public class StyleExplorer : EditorWindow
{
    private Vector2 rightScroll;
    private Vector2 leftScroll;
    private int styles;
    private string query = "";
    private int selection;
    private List<string> styleNames = new List<string>();
    private List<GUIStyle> foundStyles = new List<GUIStyle>();
    private GUIStyle style = new GUIStyle();
    private List<GUIStyle> customStyles = new List<GUIStyle>();
    private Rect textFieldRect;
    private string textToShow = "Civelek Babacık";

    private bool showSearchField = true;
    private float searchFieldWidth = 0f;
    private float curSearchFieldWidth = 0f;

    [MenuItem("Window/StyleExplorer")]
    static void Init()
    {
        StyleExplorer window = (StyleExplorer)GetWindow(typeof(StyleExplorer));
        window.Show();
    }

    void Awake()
    {
        customStyles = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).customStyles.ToList();
        foundStyles = new List<GUIStyle>(customStyles);
        styleNames = foundStyles.Select(x => x.name + " (" + customStyles.IndexOf(x) + ")").ToList();
    }

    void OnGUI()
    {

        GUISkin skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);

        GUILayout.BeginHorizontal();

        leftScroll = GUILayout.BeginScrollView(leftScroll,GUILayout.Width(510f));

        GUILayout.BeginVertical(GUILayout.Width(480f));

        GUILayout.BeginHorizontal();
        GUIStyle iconStyle = customStyles[251];
        if(GUILayout.Button(EditorGUIUtility.IconContent("Search Icon"), customStyles[355], GUILayout.Height(50f), GUILayout.Width(50f)))
        {
            searchFieldWidth = showSearchField ? 425f : 0f;
            showSearchField = showSearchField ? false : true;
        }
        GUIStyle fieldStyle = customStyles[407];
        fieldStyle.alignment = TextAnchor.MiddleLeft;
        fieldStyle.fontSize = GUI.skin.font.fontSize + 5;
        curSearchFieldWidth = Mathf.Lerp(curSearchFieldWidth, searchFieldWidth, 0.1f);
        GUI.SetNextControlName("SearchBar");
        query = GUILayout.TextField(query, fieldStyle, GUILayout.Height(50f),GUILayout.Width(curSearchFieldWidth));
        textFieldRect = GUILayoutUtility.GetLastRect();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Text to show :", GUILayout.Width(100f));
        textToShow = GUILayout.TextField(textToShow);
        GUILayout.EndHorizontal();

        GUILayout.Label("Style Name : " + style.name + " (" + customStyles.IndexOf(style) + ")", EditorStyles.boldLabel);

        if(GUI.GetNameOfFocusedControl() == "SearchBar")
        {
            foundStyles = customStyles.FindAll(x => (x.name + " (" + customStyles.IndexOf(x) + ")").Contains(query, StringComparison.OrdinalIgnoreCase));
            styleNames = foundStyles.Select(x => x.name + " (" + customStyles.IndexOf(x) + ")").ToList();
        }
        selection = GUILayout.SelectionGrid(selection, styleNames.ToArray(), 1);
        if (foundStyles.Count>0)
        {
            if (selection > foundStyles.Count - 1)
                selection = 0;
            style = foundStyles[selection];
        }
        if (Event.current.keyCode == KeyCode.Return || (Event.current.type == EventType.MouseDown && !textFieldRect.Contains(Event.current.mousePosition)))
        {
            GUI.FocusControl(null);
            Repaint();
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.DownArrow)
            {
                selection++;
                if (selection > foundStyles.Count - 1)
                    selection = foundStyles.Count - 1;
                leftScroll.y = selection * 21f - 300f;
                Event.current.Use();
            }
            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                selection--;
                if (selection < 0)
                    selection = 0;
                leftScroll.y = selection * 21f - 300f;
                Event.current.Use();
            }
        }

        //foreach(GUIStyle curStyle in foundStyles)
        //{

        //}
        GUILayout.EndVertical();

        GUILayout.EndScrollView();

        GUILayout.BeginVertical();
        //32,33,58,425 ez shadow,457
        rightScroll = GUILayout.BeginScrollView(rightScroll);

        GUILayout.Button(textToShow + "\n" + textToShow + "\n" + textToShow, style);
        GUILayout.Label(textToShow, style);
        GUILayout.Toolbar(0, new[] { textToShow, textToShow }, style);
        GUILayout.Box(textToShow + "\n" + textToShow + "\n" + textToShow + "\n" + textToShow, style);
        GUILayout.Box(textToShow, style, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Box(textToShow, style, GUILayout.Width(250), GUILayout.Height(300));
        GUILayout.Toggle(true, textToShow);
        GUILayout.Toggle(false, textToShow);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();

        Repaint();
    }
}