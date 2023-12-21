using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    private readonly Vector2 DefaultNodeSize = new Vector2(200f, 150f);
    
    public DialogGraphView()
    {
        
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode());
    }

    #region create node
    
    //generate entry node
    DialogNode GenerateEntryPointNode()
    {
        var entryNode = new DialogNode
        {
            title = "Entry Node",
            GUID = Guid.NewGuid().ToString(),
            DialogText = "ENTRYPOINT",
            EntryPoint = true,
        };

        var port = GeneratePort(entryNode, Direction.Output);

        port.portName = "Next";
        
        entryNode.outputContainer.Add(port);
        
        entryNode.RefreshExpandedState();

        entryNode.RefreshPorts();
        
        entryNode.SetPosition(new Rect(100f, 100f, 100f, 100f));

        return entryNode;
    }
    
    public void CreateNode(string _nodeName = "DialogNode")
    {
        AddElement(CreateDialogNode(_nodeName));
    }
    
    DialogNode CreateDialogNode(string _nodeName = "Dialog Node")
    {
        var node = new DialogNode
        {
            title = _nodeName,
            DialogText = _nodeName,
            GUID = Guid.NewGuid().ToString(),
            EntryPoint = false,
        };

        var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        
        node.inputContainer.Add(inputPort);

        var button = new Button(() => AddChoicePort(node));
        
        button.text = "new choice";
        
        node.titleContainer.Add(button);
        
        node.RefreshExpandedState();

        node.RefreshPorts();
        
        node.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return node;
    }
    
    //generate port
    Port GeneratePort(DialogNode _node, Direction _portDir, Port.Capacity _cap = Port.Capacity.Single)
    {
        return _node.InstantiatePort(Orientation.Horizontal, _portDir, _cap, typeof(float));
    }
    
    //ports: GetCompatiblePorts
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if(startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });


        return compatiblePorts;
    }

    void AddChoicePort(DialogNode _dialogNode)
    {
        var genPort = GeneratePort(_dialogNode, Direction.Output);

        var count = _dialogNode.outputContainer.Query("connector").ToList().Count;

        genPort.portName = $"Choice {count}";
        
        _dialogNode.outputContainer.Add(genPort);
        
        _dialogNode.RefreshExpandedState();

        _dialogNode.RefreshPorts();

    }
    
    #endregion
}
