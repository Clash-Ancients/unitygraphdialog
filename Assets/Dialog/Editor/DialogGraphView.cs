using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    public DialogGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode());
    }

    DialogNode GenerateEntryPointNode()
    {
        var entryNode = new DialogNode
        {
            title = "Entry Node",
            GUID = Guid.NewGuid().ToString(),
            DialogText = "ENTRYPOINT",
            EntryPoint = true,
        };
        
        entryNode.SetPosition(new Rect(100f, 100f, 100f, 100f));

        return entryNode;
    }
}
