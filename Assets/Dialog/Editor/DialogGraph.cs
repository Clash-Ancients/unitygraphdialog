using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraph : EditorWindow
{
    [MenuItem("Graph/Dialog Graph")]
    public static void OpenDialogGraphWindow()
    {
        var win = GetWindow<DialogGraph>();
        win.titleContent = new GUIContent("Dialogue Graph");
    }

    private DialogGraphView _mDialogGraphView;
    
    #region 系统接口
    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_mDialogGraphView);
    }
    #endregion
    
    #region 构建graphview
    
    //construct graph view
    void ConstructGraphView()
    {
        _mDialogGraphView = new DialogGraphView
        {
            name = "Dialog Graph",
        };
        
        _mDialogGraphView.StretchToParentSize();
        
        rootVisualElement.Add(_mDialogGraphView);
    }
    
    //generate tool bar
    void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var nodeCreateBtn = new Button(() => { _mDialogGraphView.CreateNode();})
        {
            text = "Create Node"
        };

        toolbar.Add(nodeCreateBtn);
        
        rootVisualElement.Add(toolbar);
        
    }
    
    #endregion
}
