using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogGraphView mDialogGraphView;

    private EditorWindow mEditorWindow;
    
    public void Init(DialogGraphView _dialoggraph, EditorWindow editwin)
    {
        mDialogGraphView = _dialoggraph;

        mEditorWindow = editwin;
    }
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialog"), 1),
            new SearchTreeEntry(new GUIContent("Dialog Node"))
            {
                userData = "DialogNode", level = 2,
            },
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = mEditorWindow.rootVisualElement.ChangeCoordinatesTo(
            mEditorWindow.rootVisualElement.parent,
            context.screenMousePosition - mEditorWindow.position.position
            );

        var localMousePosition = mDialogGraphView.contentContainer.WorldToLocal(worldMousePosition);
        
        switch (SearchTreeEntry.userData)
        {
            case "DialogNode":
            {
                mDialogGraphView.CreateNode("Dialog Node", localMousePosition);
                break;
            }
        }
        
        return true;
    }
}
