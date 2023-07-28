using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using EmiteInnaACT;
using System.ComponentModel;

public class SpellEditorWindow : EditorWindow
{
    VisualElement root;
    const string basepath = "Assets/EmiteInnaActSystemV" + "0.3" + "/Toolkit";
    const string spellscene = basepath + "/TestScene.unity";
    [MenuItem("SpellEditor/UI Toolkit/SpellEditorWindow")]
    public static void ShowExample()
    {
        SpellEditorWindow wnd = GetWindow<SpellEditorWindow>();
        wnd.titleContent = new GUIContent("SpellEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        root = rootVisualElement;

        //// VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);
        //Button button = new Button();
        //button.text = "笑死，我能用中文了耶。";
        //root.Add(button);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EmiteInnaActSystemV0.3/Toolkit/SpellEditorWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        InitTopMenu();
        InitTimeLine();
        InitializeTrack();
        //// A stylesheet can be added to a VisualElement.
        //// The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EmiteInnaActSystemV0.3/Toolkit/SpellEditorWindow.uss");
        //VisualElement labelWithStyle = new Label("Hello World! With Style");
        //labelWithStyle.styleSheets.Add(styleSheet);
        //root.Add(labelWithStyle);
    }
    #region TopMenu
    string oldScenePath = "";
    GameObject gameObject;
    SpellConfigureBase configure;
    private Button EnterDebugScene;
    private Button BackToLastScene;
    private Button SpellUse;
    private Button ShowTip;
    private Button Refresh;
    private ObjectField Character;
    private ObjectField Configure;
    void InitTopMenu()
    {
        EnterDebugScene = root.Q<Button>(nameof(EnterDebugScene));
        EnterDebugScene.clicked += () =>
        {
            string s = EditorSceneManager.GetActiveScene().path;
            if (s == spellscene) return;
            oldScenePath = s;
            if (oldScenePath != spellscene)
                EditorSceneManager.OpenScene(spellscene);
        };
        BackToLastScene = root.Q<Button>(nameof(BackToLastScene));
        BackToLastScene.clicked += () =>
        {
            string tmp = oldScenePath;
            oldScenePath = EditorSceneManager.GetActiveScene().path;
            EditorSceneManager.OpenScene(tmp);
        };
        SpellUse = root.Q<Button>(nameof(SpellUse));
        SpellUse.clicked += () =>
        {
            if (gameObject != null)
            {
                CharacterControllerBase c = gameObject.GetComponent<CharacterControllerBase>();
                if (c != null)
                {
                    if (configure != null)
                    {
                        c.CharacterApplySpell(configure.Name);
                    }
                }
            }
        };
        ShowTip = root.Q<Button>(nameof(ShowTip));
        ShowTip.clicked += () =>
        {
            if (configure != null)
            {
                Selection.activeObject = configure;
            }
        };
        Refresh = root.Q<Button>(nameof(Refresh));
        Refresh.clicked += () =>
        {
            stepSize = 15f;
            stepRealLength = 0.2f;
        };
        Character = root.Q<ObjectField>(nameof(Character));
        Character.RegisterValueChangedCallback(CharacterChangedCallBack);
        Configure = root.Q<ObjectField>(nameof(Configure));
        Configure.RegisterValueChangedCallback(ConfigureChangedCallBack);
        ShowTip = root.Q<Button>(nameof(ShowTip));
    }
    void CharacterChangedCallBack(ChangeEvent<UnityEngine.Object> evt)
    {
        string current = EditorSceneManager.GetActiveScene().path;
        if (current != spellscene) return;
        if (gameObject != null) DestroyImmediate(gameObject);
        if (evt.newValue == null) return;
        gameObject = Instantiate(evt.newValue as GameObject);
    }
    void ConfigureChangedCallBack(ChangeEvent<UnityEngine.Object> evt)
    {
        configure = evt.newValue as SpellConfigureBase;
    }

    #endregion

    #region TimeLine
    public float stepSize;
    public float stepRealLength = 0.2f;
    int block = 5;
    float bblock = 10;
    IMGUIContainer timeline;
    ScrollView contentView;
    VisualElement contentContainer;
    VisualElement contentRight;
    float scrollOfffset { get => Mathf.Abs(contentContainer.transform.position.x); }
    void InitTimeLine()
    {
        timeline = root.Q<IMGUIContainer>("TimeLine");
        timeline.onGUIHandler = DrawTimeLine;
        stepSize = 15f;
        stepRealLength = 0.2f;
        block = 5;
        bblock = 10;
        contentView = root.Q<ScrollView>("ContentView");
        contentContainer = contentView.Q<VisualElement>("unity-content-container");
        contentRight = root.Q<VisualElement>("ContentRight");
        contentRight.RegisterCallback<WheelEvent>(TimeWheelEvent);
    }
    void TimeWheelEvent(WheelEvent evt)
    {
        float delta = evt.delta.y;
        stepSize -= 0.5f*delta;
        stepSize = Mathf.Clamp(stepSize, 5f, 60f);
        timeline.MarkDirtyLayout();
    }
    float frame2pos(float frame)
    {
        float i = frame / stepRealLength;
        float p = i * stepSize - (int)(scrollOfffset / bblock) * stepSize - 1.0f * (scrollOfffset % bblock) * stepSize / bblock;
//        float p = (i - (int)(scrollOfffset / bblock)) * stepSize -(int)(scrollOfffset % bblock) * stepSize / bblock;
        return p+scrollOfffset;
    }
    float pos2frame(float pos)
    {
        float i=(pos+(int)(scrollOfffset%bblock)*stepSize/bblock)/stepSize+(int)(scrollOfffset / bblock);
        return i * stepRealLength;
    }
    void DrawTimeLine()
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Rect rect = timeline.contentRect;
        int index = (int)(scrollOfffset/bblock);
        float yu = (scrollOfffset % bblock);
        float offset = -1.0f*yu * stepSize/bblock;
        float buf = (stepSize >= 60) ? 0.5f : 1f;
        for(float i = index,pls=0; pls < 100;pls+=buf,i+=buf)
        {
            float xpos = pls * stepSize;
            if (xpos + offset >= 0)
            {
                if (i % block < 0.5f)
                {

                    Handles.DrawLine(new Vector3(xpos + offset, 0), new Vector3(xpos + offset, 50));
                    string txt = (i * stepRealLength).ToString();
                    GUI.Label(new Rect(xpos + offset - txt.Length + 3, 10, 33, 45), txt);
                }
                else if (stepSize >= 30)
                {
                    Handles.DrawLine(new Vector3(xpos + offset, 0), new Vector3(xpos + offset, 20));
                    string txt = (i * stepRealLength).ToString();
                    GUI.Label(new Rect(xpos + offset - txt.Length + 3, 10, 33, 45), txt);
                }
                else
                {
                    Handles.DrawLine(new Vector3(xpos + offset, 0), new Vector3(xpos + offset, 20));
                }
            }
        }

        Handles.EndGUI();
    }

    #endregion

    #region Track
    VisualElement animationLine;
    IMGUIContainer animationfield;
    void InitializeTrack()
    {
        animationLine = root.Q<VisualElement>("AnimationLine");
        animationfield = root.Q<IMGUIContainer>("AnimationField");
        animationfield.onGUIHandler = DrawAnimationField;
    }
    void DrawAnimationField()
    {
        if (configure == null) return;
        float linewidth=40;
        Handles.BeginGUI();
        //动画
        Handles.color = Color.green;
        for (int i = 0; i < configure.animationEvent.Count; i++)
        {
            SpellAnimationEvent a = configure.animationEvent[i];
            Handles.DrawSolidRectangleWithOutline(new Rect(frame2pos(a.happenTime*configure.timeMultiplier), 0, frame2pos(a.clip.length * configure.timeMultiplier + a.happenTime * configure.timeMultiplier) -frame2pos(a.happenTime * configure.timeMultiplier), linewidth), Handles.color, Handles.color);
            string txt = a.clip.name;
            Color txtColor = new Color(25,180,25);
            GUIStyle style = GUI.skin.box;
            style.normal.textColor = txtColor;
            GUI.Label(new Rect(frame2pos(a.happenTime * configure.timeMultiplier) + 5, 10, txt.Length*15, 25), txt,style);
        }
        //脚本事件
        Handles.color = new Color(75.0f/256,75.0f/256,180.0f/256);
        for (int i = 0; i < configure.scriptEvents.Count; i++)
        {
            SpellScriptEvent a = configure.scriptEvents[i];
            Handles.DrawSolidRectangleWithOutline(new Rect(frame2pos(a.happenTime * configure.timeMultiplier), +linewidth,7, linewidth), Handles.color, Handles.color);
            string txt = a.eventName ;
            Color txtColor = new Color(25, 180, 25);
            GUIStyle style = GUI.skin.box;
            style.normal.textColor = txtColor;
            GUI.Label(new Rect(frame2pos(a.happenTime * configure.timeMultiplier) + 5, 10 + linewidth, txt.Length * 15, 25), txt, style);
        }
        //音效事件
        Handles.color = new Color(75.0f / 256, 164.0f / 256, 175.0f / 256);
        for (int i = 0; i < configure.audioEvents.Count; i++)
        {
            SpellAudioEvent a = configure.audioEvents[i];
            Handles.DrawSolidRectangleWithOutline(new Rect(frame2pos(a.happenTime * configure.timeMultiplier), +linewidth*2, frame2pos(a.clip.length*configure.timeMultiplier + a.happenTime * configure.timeMultiplier) - frame2pos(a.happenTime * configure.timeMultiplier), linewidth), Handles.color, Handles.color);
            string txt = a.clip.name;
            Color txtColor = new Color(25, 180, 25);
            GUIStyle style = GUI.skin.box;
            style.normal.textColor = txtColor;
            GUI.Label(new Rect(frame2pos(a.happenTime * configure.timeMultiplier) + 5, 10 + linewidth*2, txt.Length * 15, 25), txt, style);
        }
        //区域伤害事件
        Handles.color = new Color(22.0f / 256, 221.0f / 256, 175.0f / 256);
        for (int i = 0; i < configure.attackEvents.Count; i++)
        {
            SpellAttackEvent a = configure.attackEvents[i];
            Handles.DrawSolidRectangleWithOutline(new Rect(frame2pos(a.happenTime * configure.timeMultiplier), +linewidth * 3, 7, linewidth), Handles.color, Handles.color);
            string txt = a.eventName;
            Color txtColor = new Color(25, 180, 25);
            GUIStyle style = GUI.skin.box;
            style.normal.textColor = txtColor;
            GUI.Label(new Rect(frame2pos(a.happenTime * configure.timeMultiplier) + 5, 10 + linewidth * 3, txt.Length * 15, 25), txt, style);
        }
        Handles.EndGUI();
    }
    #endregion
}