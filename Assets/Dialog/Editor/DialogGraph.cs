using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

    private string _mFileName = "New DialogGraph";
    
    #region 系统接口
    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateBlackBoard();
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


        var fileNameText = new TextField
        {
            value = _mFileName,
        };
        fileNameText.SetValueWithoutNotify(_mFileName);
        fileNameText.MarkDirtyRepaint();
        fileNameText.RegisterValueChangedCallback((evt) => _mFileName = evt.newValue);
        
        toolbar.Add(fileNameText);

        var saveBtn = new Button(() => { RequestBtnOperation(true);}){ text = "Save Graph"};
        var loadBtn = new Button(() => { RequestBtnOperation(false);}){ text = "Load Graph"};
        
        toolbar.Add(saveBtn);
        toolbar.Add(loadBtn);
        
        var nodeCreateBtn = new Button(() => { _mDialogGraphView.CreateNode();})
        {
            text = "Create Node"
        };

        toolbar.Add(nodeCreateBtn);
        
        rootVisualElement.Add(toolbar);
        
    }

    void RequestBtnOperation(bool save)
    {
        if (string.IsNullOrEmpty(_mFileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "please enter valid name", "ok");
            return;
        }

        var utilityInst = GraphSaveUtility.GetInstance(_mDialogGraphView);

        if (save) 
            utilityInst.SaveGraph(_mFileName);
        else 
            utilityInst.LoadGraph(_mFileName);
        
    }
    
    #endregion
    
    #region black board
    void GenerateBlackBoard()
    {
        var blackboard = new Blackboard(_mDialogGraphView);
        
        blackboard.Add(new BlackboardSection {title = "Exposed Properties"});
        
        _mDialogGraphView.Add(blackboard);
    }
    #endregion
}
