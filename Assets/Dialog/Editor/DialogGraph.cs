using System;
using UnityEditor;
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
    
    private void OnEnable()
    {
        _mDialogGraphView = new DialogGraphView
        {
            name = "Dialog Graph",
        };
        
        _mDialogGraphView.StretchToParentSize();
        
        rootVisualElement.Add(_mDialogGraphView);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_mDialogGraphView);
    }
}
